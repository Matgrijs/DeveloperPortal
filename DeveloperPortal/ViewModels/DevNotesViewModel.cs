using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DeveloperPortal.Models.Notes;
using DeveloperPortal.Services;
using Newtonsoft.Json;

namespace DeveloperPortal.ViewModels
{
    public class DevNotesViewModel : ObservableObject
    {
        private readonly ApiService _apiService;
        private static readonly HttpClient HttpClient = new ();

        public DevNotesViewModel(ApiService apiService)
        {
            _apiService = apiService;
            Notes = new ObservableCollection<Note>();
            SaveNoteCommand = new AsyncRelayCommand(CreateOrUpdateNoteAsync);
            DeleteNoteCommand = new AsyncRelayCommand<Note>(OnDeleteNoteAsync);
        }

        public ObservableCollection<Note> Notes { get; }

        private string _noteContent = null!;
        public string NoteContent
        {
            get => _noteContent;
            set => SetProperty(ref _noteContent, value);
        }

        private Note? _selectedNote;
        public Note? SelectedNote
        {
            get => _selectedNote;
            set
            {
                if (SetProperty(ref _selectedNote, value))
                {
                    NoteContent = _selectedNote?.Content!;
                }
            }
        }

        public ICommand SaveNoteCommand { get; }
        public ICommand DeleteNoteCommand { get; }

        public async Task LoadNotesAsync()
        {
            try
            {
                var url = $"{_apiService.BaseUrl}/api/Note";
                var response = await HttpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var notes = JsonConvert.DeserializeObject<List<Note>>(json);
                    Notes.Clear();
                    if (notes != null)
                        foreach (var note in notes)
                        {
                            Notes.Add(note);
                        }
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
            {
                Debug.WriteLine("we zijn aan het updaten");
                await OnEditNoteAsync(SelectedNote);
            }
            else
            {
                Debug.WriteLine("we zijn aan het aanmaken");
                await OnCreateNoteAsync();
            }
        }

        private async Task OnCreateNoteAsync()
        {
            if (!string.IsNullOrWhiteSpace(NoteContent))
            {
                var newNote = new CreateNoteDto
                {
                    Username = AuthenticationService.Instance.UserName,
                    Auth0Id = AuthenticationService.Instance.Auth0Id,
                    Content = NoteContent,
                };

                var json = JsonConvert.SerializeObject(newNote);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                try
                {
                    var response = await HttpClient.PostAsync($"{_apiService.BaseUrl}/api/Note", content);
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
            if (string.IsNullOrWhiteSpace(NoteContent))
            {
                Console.WriteLine($"No note is selected or content is empty {NoteContent.Length}");
                return;
            }

            var updatedNote = new UpdateNoteDto(note.Id, note.Username, note.Auth0Id, NoteContent);
            Console.WriteLine($"update object {updatedNote}");
            var json = JsonConvert.SerializeObject(updatedNote);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await HttpClient.PutAsync($"{_apiService.BaseUrl}/api/Note/{note.Id}", content);
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
                var response = await HttpClient.DeleteAsync($"{_apiService.BaseUrl}/api/Note/{note.Id}");
                if (response.IsSuccessStatusCode)
                {
                    Notes.Remove(note);
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }
    }
}