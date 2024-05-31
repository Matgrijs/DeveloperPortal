using System.Collections.ObjectModel;
using System.Text;
using DeveloperPortal.Models.Notes;
using DeveloperPortal.Services;
using Newtonsoft.Json;

namespace DeveloperPortal;

public partial class DevNotes
{
    private ObservableCollection<Note> Notes { get; } = new ObservableCollection<Note>();
    private readonly NavigationService _navigationService;
    private readonly ApiService _apiService;
    private static readonly HttpClient HttpClient = new HttpClient();

    public DevNotes(NavigationService navigationService, ApiService apiService)
    {
        InitializeComponent();
        _navigationService = navigationService;
        _apiService = apiService;

        NoteList.ItemsSource = Notes;
        LoadNotes();
    }

    private async void LoadNotes()
    {
        try
        {
            var notes = await LoadNotesAsync();
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Notes.Clear();
                if (notes != null)
                    foreach (var note in notes)
                    {
                        Notes.Add(note);
                    }
            });
        }
        catch (Exception ex)
        {
            SentrySdk.CaptureException(ex);
        }
    }

    private async Task<List<Note>?> LoadNotesAsync()
    {
        try
        {
            string url = $"{_apiService.BaseUrl}/api/Note";
            var response = await HttpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Note>>(json);
            }
        }
        catch (Exception ex)
        {
            SentrySdk.CaptureException(ex);
        }
        return null;
    }

    private async void OnCreateNote(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(NoteContent.Text))
        {
            var newNote = new CreateNoteDto
            {
                Username = AuthenticationService.Instance.UserName,
                Auth0Id = AuthenticationService.Instance.Auth0Id,
                Content = NoteContent.Text,
            };

            var json = JsonConvert.SerializeObject(newNote);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await HttpClient.PostAsync($"{_apiService.BaseUrl}/api/Note", content);
                if (response.IsSuccessStatusCode)
                {
                    NoteContent.Text = string.Empty;
                    await LoadNotesAsync();
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }
    }

    private void OnNoteSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Note selectedNote)
        {
            NoteContent.Text = selectedNote.Content;
        }
    }

    private async void OnEditNote(object sender, EventArgs e)
    {
        if (sender is not Button { CommandParameter: Note existingNote }) return;

        var updatedNote = new UpdateNoteDto(existingNote.Id, existingNote.Username, existingNote.Auth0Id, NoteContent.Text);

        var json = JsonConvert.SerializeObject(updatedNote);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            var response = await HttpClient.PutAsync($"{_apiService.BaseUrl}/api/Note/{existingNote.Id}", content);
            if (response.IsSuccessStatusCode)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Notes[Notes.IndexOf(existingNote)] = new Note(updatedNote.Id, updatedNote.Username, updatedNote.Auth0Id, updatedNote.Content);
                });
            }
        }
        catch (Exception ex)
        {
            SentrySdk.CaptureException(ex);
        }
    }

    private async void OnDeleteNote(object sender, EventArgs e)
    {
        if (sender is not Button { CommandParameter: Note note }) return;

        try
        {
            var response = await HttpClient.DeleteAsync($"{_apiService.BaseUrl}/api/Note/{note.Id}");
            if (response.IsSuccessStatusCode)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Notes.Remove(note);
                });
            }
        }
        catch (Exception ex)
        {
            SentrySdk.CaptureException(ex);
        }
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await _navigationService.OnBackButtonClickedAsync();
    }
}
