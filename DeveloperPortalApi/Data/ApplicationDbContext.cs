using DeveloperPortalApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeveloperPortalApi.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<ChatMessage> ChatMessages { get; set; }
    
    public DbSet<Note> Notes { get; set; }
    
    public DbSet<PokerVote> PokerVotes { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<ChatMessage>().HasData(DefaultDbData.ChatMessages);
        modelBuilder.Entity<Note>().HasData(DefaultDbData.Notes);
    }
}

