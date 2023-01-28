using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using OpenHealthTrackerApi.Data;
using OpenHealthTrackerApi.Data.Models;

namespace OpenHealthTrackerApi.Services.DAL;

public class EmotionDbService : IEmotionDbService
{
    private readonly OHTDbContext _db;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly Guid _user;

    public EmotionDbService(OHTDbContext db, IHttpContextAccessor httpContextAccessor)
    {
        _db = db;
        _httpContextAccessor = httpContextAccessor;
        _user = new Guid(_httpContextAccessor.HttpContext.User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);
    }

    public async Task<Emotion[]> GetEmotionsByIdsAsync(int[]? ids)
    {
        if (ids == null) return Array.Empty<Emotion>();
        var emotions = await _db.Emotions.Where(x => ids.Contains(x.Id)).ToListAsync();

        if (ids.Any(x => emotions.All(y => x != y.Id && y.UserId == _user))) throw new KeyNotFoundException("Emotion not found");

        return emotions.ToArray();
    }

    public async Task<Emotion[]> GetEmotionsByUserAsync()
    {
        return await _db.Emotions.Where(x => x.UserId == _user).ToArrayAsync();
    }

    public async Task<EmotionCategory[]> GetEmotionCategoriesByUserAsync()
    {
        return await _db.EmotionCategories.Where(x => x.User == _user).ToArrayAsync();
    }

    public async Task<int> CreateEmotionCategoryAsync(string name)
    {
        var emotion = new EmotionCategory
        {
            User = _user,
            Name = name
        };
        await _db.EmotionCategories.AddAsync(emotion);
        await _db.SaveChangesAsync();
        return emotion.Id;
    }

    public async Task<int> CreateEmotionAsync(string name, int categoryId)
    {
        var emotion = new Emotion
        {
            CategoryId = categoryId,
            Name = name,
            UserId = _user
        };

        await _db.Emotions.AddAsync(emotion);
        await _db.SaveChangesAsync();
        return emotion.Id;
    }
}