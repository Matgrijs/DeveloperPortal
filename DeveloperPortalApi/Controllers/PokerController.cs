using DeveloperPortalApi.Data;
using DeveloperPortalApi.Entities;
using DeveloperPortalApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeveloperPortalApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PokerController(ApplicationDbContext context, PokerService pokerService) : ControllerBase
{
    // Get all poker votes
    [HttpGet]
    public async Task<ActionResult<List<PokerVote>>> GetAllPokerVotes()
    {
        var pokerVotes = await context.PokerVotes.ToListAsync();
        return pokerVotes;
    }

    [HttpPost]
    public async Task<IActionResult> PostVote(PokerVote pokerVote)
    {
        if (string.IsNullOrEmpty(pokerVote.Username) || string.IsNullOrEmpty(pokerVote.Vote))
            return BadRequest("Username and vote are required.");

        await pokerService.AddVote(pokerVote);

        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateVote(Guid id, PokerVote pokerVote)
    {
        if (id != pokerVote.Id) return BadRequest("Poker value ID mismatch");

        var existingVote = await context.PokerVotes.AsNoTracking().FirstOrDefaultAsync(n => n.Id == id);
        if (existingVote == null) return NotFound();

        await pokerService.UpdateVote(pokerVote);
        return NoContent();
    }
}