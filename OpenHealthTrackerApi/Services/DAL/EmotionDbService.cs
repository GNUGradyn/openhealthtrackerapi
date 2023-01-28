using Microsoft.EntityFrameworkCore;
using OpenHealthTrackerApi.Data;
using OpenHealthTrackerApi.Data.Models;

namespace OpenHealthTrackerApi.Services.DAL;

public class EmotionDbService : IEmotionDbService
{
    private readonly OHTDbContext _db;

    public EmotionDbService(OHTDbContext db)
    {
        _db = db;
    }

    public async Task<Emotion[]> GetEmotionsByIdsAsync(int[]? ids)
    {
        if (ids == null) return Array.Empty<Emotion>();
        var emotions = await _db.Emotions.Where(x => ids.Contains(x.Id)).ToListAsync();

        if (ids.Any(x => emotions.All(y => x != y.Id))) throw new KeyNotFoundException("Emotion not found");

        return emotions.ToArray();
    }

    public async Task<Emotion[]> GetEmotionsByUserAsync(Guid user)
    {
        return await _db.Emotions.Where(x => x.UserId == user).ToArrayAsync();
    }

    public async Task<EmotionCategory[]> GetEmotionCategoriesByUserAsync(Guid user)
    {
        return await _db.EmotionCategories.Where(x => x.User == user).ToArrayAsync();
    }

    public async Task<int> CreateEmotionCategoryAsync(string name, Guid user)
    {
        var emotion = new EmotionCategory
        {
            User = user,
            Name = name
        };
        await _db.EmotionCategories.AddAsync(emotion);
        await _db.SaveChangesAsync();
        return emotion.Id;
    }

    public async Task<int> CreateEmotionAsync(string name, int categoryId, Guid user)
    {
        var emotion = new Emotion
        {
            CategoryId = categoryId,
            Name = name,
            UserId = user
        };

        await _db.Emotions.AddAsync(emotion);
        await _db.SaveChangesAsync();
        return emotion.Id;
    }
}