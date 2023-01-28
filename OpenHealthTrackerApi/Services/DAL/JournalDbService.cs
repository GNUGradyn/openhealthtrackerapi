using OpenHealthTrackerApi.Data;
using OpenHealthTrackerApi.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace OpenHealthTrackerApi.Services.DAL;

public class JournalDbService : IJournalDbService
{
    private readonly OHTDbContext _db;

    public JournalDbService(OHTDbContext db)
    {
        _db = db;
    }

    public async Task<int> CreateEntryAsync(string text, Emotion[] emotions, Activity[] activities, Guid user)
    {
        using (var dbContextTransaction = _db.Database.BeginTransaction())
        {
            // Add journal entry
            var entry = await _db.JournalEntries.AddAsync(new JournalEntry()
            {
                Body = text,
                CreatedAt = DateTime.UtcNow,
                UserId = user
            });
            await _db.SaveChangesAsync(); // Save so we have access to the ID

            // Attach emotions
            await _db.EmotionEntries.AddRangeAsync(emotions.Select(x => new EmotionEntry()
            {
                EmotionId = x.Id,
                JournalEntryId = entry.Entity.Id
            }));

            // Attach activities
            await _db.ActivityEntries.AddRangeAsync(activities.Select(x => new ActivityEntry()
            {
                ActivityId = x.Id,
                JournalEntryId = entry.Entity.Id
            }));

            await _db.SaveChangesAsync();
            await dbContextTransaction.CommitAsync();
            return entry.Entity.Id;
        }
    }
    
    public async Task<JournalEntry[]> GetEntriesAsync(int count, int start, Guid user)
    {
        return await _db.JournalEntries.Where(x => x.UserId == user).Skip(start).Take(count).ToArrayAsync();
    }
}
