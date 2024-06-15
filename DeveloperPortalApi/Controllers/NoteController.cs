using DeveloperPortalApi.Data;
using DeveloperPortalApi.Entities;
using DeveloperPortalApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeveloperPortalApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NoteController(ApplicationDbContext context, NoteService noteService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Note>>> GetAllNotes()
    {
        return await context.Notes.ToListAsync();
    }

    [HttpPost]
    public async Task<IActionResult> PostNote(Note note)
    {
        if (string.IsNullOrEmpty(note.Username) || string.IsNullOrEmpty(note.Content))
            return BadRequest("Username and content are required.");
        await noteService.AddNote(note);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateNote(Guid id, Note note)
    {
        if (id != note.Id) return BadRequest("Note ID mismatch");

        var existingNote = await context.Notes.AsNoTracking().FirstOrDefaultAsync(n => n.Id == id);
        if (existingNote == null) return NotFound();


        await noteService.UpdateNote(note);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNote(Guid id)
    {
        var note = await context.Notes.FindAsync(id);
        if (note == null) return NotFound();

        await noteService.DeleteNote(id);
        return NoContent();
    }
}