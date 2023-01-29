using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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

    public async Task<Emotion[]> GetEmotionsByUserAsync() // TODO: replace return type with list instead of array
    {
        return await _db.Emotions.Where(x => x.UserId == _user).ToArrayAsync();
    }

    public async Task<List<Models.EmotionCategory>> GetEmotionCategoriesByUserAsync(bool includeEmotions = true)
    {
        List<EmotionCategory> results;
        if (includeEmotions)
            results = await _db.EmotionCategories.Include(x => x.emotions).Where(x => x.User == _user).ToListAsync();
        else results = await _db.EmotionCategories.Where(x => x.User == _user).ToListAsync();
        return results.Select(x => new Models.EmotionCategory
        {
            Name = x.Name,
            Id = x.Id,
            emotions = x.emotions.Select(y => new Models.Emotion()
            {
                Name = y.Name,
                Id = y.Id
            }).ToList()
        }).ToList();
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