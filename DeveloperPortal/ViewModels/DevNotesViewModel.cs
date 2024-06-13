using System.Collections.ObjectModel;
using System.Globalization;
using System.Resources;
using System.Text;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DeveloperPortal.Models.Notes;
using DeveloperPortal.Resources;
using DeveloperPortal.Services;
using DeveloperPortal.Services.DevHttpsConnectionHelper;
using Newtonsoft.Json;

namespace DeveloperPortal.ViewModels;

public class DevNotesViewModel : ObservableObject
{
    private readonly IDevHttpsConnectionHelper _httpsHelper;
    private readonly ResourceManager _resourceManager = new(typeof(AppResources));

    private string _noteContent = null!;

    private Note? _selectedNote;

    public DevNotesViewModel(IDevHttpsConnectionHelper httpsHelper)
    {
        _httpsHelper = httpsHelper;
        Notes = new ObservableCollection<Note>();
        SaveNoteCommand = new AsyncRelayCommand(CreateOrUpdateNoteAsync);
        DeleteNoteCommand = new AsyncRelayCommand<Note>(OnDeleteNoteAsync);
    }

    public string? TitleLabel => _resourceManager.GetString("NotesTitle", CultureInfo.CurrentCulture);
    public string? SaveLabel => _resourceManager.GetString("Save", CultureInfo.CurrentCulture);
    public string? PlaceHolderLabel => _resourceManager.GetString("AddDescription", CultureInfo.CurrentCulture);
    public string? DeleteLabel => _resourceManager.GetString("Delete", CultureInfo.CurrentCulture);

    public ObservableCollection<Note> Notes { get; }

    public string NoteContent
    {
        get => _noteContent;
        set => SetProperty(ref _noteContent, value);
    }

    public Note? SelectedNote
    {
        get => _selectedNote;
        set
        {
            if (SetProperty(ref _selectedNote, value)) NoteContent = _selectedNote?.Content!;
        }
    }

    public ICommand SaveNoteCommand { get; }
    public ICommand DeleteNoteCommand { get; }

    public async Task LoadNotesAsync()
    {
        try
        {
            var httpClient = _httpsHelper.HttpClient;
            var url = $"{_httpsHelper.DevServerRootUrl}/api/Note";
            var response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var notes = JsonConvert.DeserializeObject<List<Note>>(json);
                Notes.Clear();
                if (notes != null)
                    foreach (var note in notes)
                        Notes.Add(note);
            }
        }
        catch (Exception ex)
        {
            SentrySdk.CaptureException(ex);
        }
    }

    private async Task CreateOrUpdateNoteAsync()
    {
        if (SelectedNote != null)
            await OnEditNoteAsync(SelectedNote);
        else
            await OnCreateNoteAsync();
    }

    private async Task OnCreateNoteAsync()
    {
        if (!string.IsNullOrWhiteSpace(NoteContent))
        {
            var newNote = new CreateNoteDto
            {
                Username = AuthenticationService.Instance.UserName,
                Auth0Id = AuthenticationService.Instance.Auth0Id,
                Content = NoteContent
            };

            var json = JsonConvert.SerializeObject(newNote);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var httpClient = _httpsHelper.HttpClient;
                var response = await httpClient.PostAsync($"{_httpsHelper.DevServerRootUrl}/api/Note", content);
                if (response.IsSuccessStatusCode)
                {
                    NoteContent = string.Empty;
                    await LoadNotesAsync();
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }
    }

    private async Task OnEditNoteAsync(Note note)
    {
        if (string.IsNullOrWhiteSpace(NoteContent)) return;

        var updatedNote = new UpdateNoteDto(note.Id, note.Username, note.Auth0Id, NoteContent);
        var json = JsonConvert.SerializeObject(updatedNote);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            var httpClient = _httpsHelper.HttpClient;
            var response = await httpClient.PutAsync($"{_httpsHelper.DevServerRootUrl}/api/Note/{note.Id}", content);
            if (response.IsSuccessStatusCode)
            {
                NoteContent = string.Empty;
                await LoadNotesAsync();
            }
        }
        catch (Exception ex)
        {
            SentrySdk.CaptureException(ex);
        }
    }

    private async Task OnDeleteNoteAsync(Note? note)
    {
        if (note == null) return;

        try
        {
            var httpClient = _httpsHelper.HttpClient;
            var response = await httpClient.DeleteAsync($"{_httpsHelper.DevServerRootUrl}/api/Note/{note.Id}");
            if (response.IsSuccessStatusCode) Notes.Remove(note);
        }
        catch (Exception ex)
        {
            SentrySdk.CaptureException(ex);
        }
    }
}