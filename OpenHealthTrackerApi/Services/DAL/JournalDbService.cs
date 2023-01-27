using OpenHealthTrackerApi.Data;
using OpenHealthTrackerApi.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace OpenHealthTrackerApi.Services.DAL;

public class JournalDbService : IJournalDbService
{
    private readonly DbFactory _dbFactory;

    public JournalDbService(DbFactory dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task CreateEntryAsync(string text, Emotion[] emotions, Activity[] activities, Guid user)
    {
        using (var db = _dbFactory.OHT())
        {
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                // Add journal entry
                var entry = await db.JournalEntries.AddAsync(new JournalEntry()
                {
                    Body = text,
                    CreatedAt = DateTime.UtcNow,
                    UserId = user
                });
                await db.SaveChangesAsync(); // Save so we have access to the ID
                
                // Attach emotions
                await db.EmotionEntries.AddRangeAsync(emotions.Select(x => new EmotionEntry()
                {
                    EmotionId = x.Id,
                    JournalEntryId = entry.Entity.Id
                }));
                
                // Attach activities
                await db.ActivityEntries.AddRangeAsync(activities.Select(x => new ActivityEntry()
                {
                    ActivityId = x.Id,
                    JournalEntryId = entry.Entity.Id
                }));

                await db.SaveChangesAsync();
                await dbContextTransaction.CommitAsync();
            }
        }
    }
}