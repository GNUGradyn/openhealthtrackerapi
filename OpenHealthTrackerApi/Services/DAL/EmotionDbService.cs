using Microsoft.EntityFrameworkCore;
using OpenHealthTrackerApi.Data;
using OpenHealthTrackerApi.Data.Models;

namespace OpenHealthTrackerApi.Services.DAL;

public class EmotionDbService : IEmotionDbService
{
    private readonly DbFactory _dbFactory;

    public EmotionDbService(DbFactory dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task<Emotion[]> GetEmotionsByIdsAsync(int[]? ids)
    {
        if (ids == null) return Array.Empty<Emotion>();
        using (var db = _dbFactory.OHT())
        {
            var emotions = await db.Emotions.Where(x => ids.Contains(x.Id)).ToListAsync();

            if (ids.Any(x => emotions.All(y => x != y.Id))) throw new KeyNotFoundException("Emotion not found");

            return emotions.ToArray();
        }
    }

    public async Task<Emotion[]> GetEmotionsByUserAsync(Guid user)
    {
        using (var db = _dbFactory.OHT())
        {
            return await db.Emotions.Where(x => x.UserId == user).ToArrayAsync();
        }
    }

    public async Task<EmotionCategory[]> GetEmotionCategoriesByUserAsync(Guid user)
    {
        using (var db = _dbFactory.OHT())
        {
            return await db.EmotionCategories.Where(x => x.User == user).ToArrayAsync();
        }
    }

    public async Task<int> CreateEmotionCategoryAsync(string name, Guid user)
    {
        var emotion = new EmotionCategory
        {
            User = user,
            Name = name
        };
        using (var db = _dbFactory.OHT())
        {
            await db.EmotionCategories.AddAsync(emotion);
            await db.SaveChangesAsync();
            return emotion.Id;
        }
    }
}