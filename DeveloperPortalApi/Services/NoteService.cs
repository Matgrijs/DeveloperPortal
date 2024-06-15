using DeveloperPortalApi.Data;
using DeveloperPortalApi.Entities;

namespace DeveloperPortalApi.Services;

public class NoteService(ApplicationDbContext dbContext)
{
    public async Task AddNote(Note note)
    {
        dbContext.Notes.Add(note);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateNote(Note note)
    {
        dbContext.Notes.Update(note);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteNote(Guid id)
    {
        var note = await dbContext.Notes.FindAsync(id);
        if (note != null)
        {
            dbContext.Notes.Remove(note);
            await dbContext.SaveChangesAsync();
        }
    }
}