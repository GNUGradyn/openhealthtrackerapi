using System.Security.Claims;
using OpenHealthTrackerApi.Data;
using OpenHealthTrackerApi.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace OpenHealthTrackerApi.Services.DAL;

public class JournalDbService : IJournalDbService
{
    private readonly OHTDbContext _db;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly Guid _user;

    public JournalDbService(OHTDbContext db, IHttpContextAccessor httpContextAccessor)
    {
        _db = db;
        _httpContextAccessor = httpContextAccessor;
        _user = new Guid(_httpContextAccessor.HttpContext.User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);
    }

    public async Task<int> CreateEntryAsync(string text, List<Models.Emotion> emotions, List<Models.Activity> activities)
    {
        using (var dbContextTransaction = _db.Database.BeginTransaction())
        {
            // Add journal entry
            var entry = await _db.JournalEntries.AddAsync(new JournalEntry()
            {
                Body = text,
                CreatedAt = DateTime.UtcNow,
                UserId = _user
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
    
    public async Task<List<Models.JournalEntry>> GetEntriesAsync(int count, int start)
    {
        var results = _db.JournalEntries.Where(x => x.UserId == _user).Skip(start).Take(count);
        return await results.Select(x => new Models.JournalEntry
        {
            CreatedAt = x.CreatedAt,
            Activities = x.Activities.Select(y => new Models.Activity
            {
                Id = y.Activity.Id,
                Name = y.Activity.Name
            }).ToList(),
            Emotions = x.Emotions.Select(x => new Models.Emotion
            {
                Id = x.Emotion.Id,
                Name = x.Emotion.Name
            }).ToList(),
            Id = x.Id
        }).ToListAsync();
    }

    public async Task DeleteEntry(int id)
    {
        var entry = await _db.JournalEntries.FindAsync(id);
        if (entry == null) throw new KeyNotFoundException();
        _db.RemoveRange(_db.EmotionEntries.Where(x => x.JournalEntryId == entry.Id));
        _db.RemoveRange(_db.ActivityEntries.Where(x => x.JournalEntryId == entry.Id));
        _db.Remove(entry);
        await _db.SaveChangesAsync();
    }
}
