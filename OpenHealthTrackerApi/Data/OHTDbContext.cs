using Microsoft.EntityFrameworkCore;
using OpenHealthTrackerApi.Data.Models;

namespace OpenHealthTrackerApi.Data;

public class OHTDbContext : DbContext
{
    private IConfiguration _config;
    
    public DbSet<Activity> Activities { get; set; }
    public DbSet<Emotion> Emotions { get; set; }
    public DbSet<JournalEntry> JournalEntries { get; set; }
    public DbSet<EmotionEntry> EmotionEntries { get; set; }
    public DbSet<ActivityEntry> ActivityEntries { get; set; }
    public DbSet<EmotionCategory> EmotionCategories { get; set; }

    public OHTDbContext(IConfiguration config)
    {
        _config = config;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql($"Host={_config.GetValue<string>("Database:Host")};Database={_config.GetValue<string>("Database:Database")};Username={_config.GetValue<string>("Database:Username")};Password={_config.GetValue<string>("Database:Password")}");
}