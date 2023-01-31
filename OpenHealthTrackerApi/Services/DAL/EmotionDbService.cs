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

    public async Task<List<Models.Emotion>> GetEmotionsByIdsAsync(int[]? ids)
    {
        if (ids == null) return new List<Models.Emotion>();
        var emotions = await _db.Emotions.Include(x => x.Category).Where(x => ids.Contains(x.Id)).ToListAsync();

        if (ids.Any(x => emotions.All(y => x != y.Id && y.UserId == _user))) throw new KeyNotFoundException("Emotion not found");

        return emotions.Select(x => new Models.Emotion
        {
            Category = new Models.EmotionCategory()
            {
                Id = x.CategoryId,
                Name = x.Category.Name,
                AllowMultiple = x.Category.AllowMultiple
            },
            Id = x.Id,
            Name = x.Name
        }).ToList();
    }

    public async Task<List<Models.Emotion>> GetEmotionsByUserAsync()
    {
        var results = _db.Emotions.Where(x => x.UserId == _user);
        return await results.Select(x => new Models.Emotion
        {
            Category = new Models.EmotionCategory
            {
                Id = x.CategoryId,
                Name = x.Category.Name,
                AllowMultiple = x.Category.AllowMultiple
            },
            Name = x.Name,
            Id = x.Id
        }).ToListAsync();
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
            AllowMultiple = x.AllowMultiple,
            emotions = x.emotions.Select(y => new Models.Emotion()
            {
                Name = y.Name,
                Id = y.Id
            }).ToList()
        }).ToList();
    }

    public async Task<int> CreateEmotionCategoryAsync(string name, bool allowMultiple = false)
    {
        var emotion = new EmotionCategory
        {
            User = _user,
            Name = name,
            AllowMultiple = allowMultiple
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

    public async Task DeleteEmotionAsync(int id)
    {
        var emotion = await _db.Emotions.FindAsync(id);
        if (emotion == null) throw new KeyNotFoundException("Emotion not found");
        _db.Remove(emotion);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteEmotionCategoryAsync(int id)
    {
        var category = await _db.EmotionCategories.Include(x => x.emotions).SingleOrDefaultAsync(x => x.Id == id);
        if (category == null) throw new KeyNotFoundException("Category not found");
        if (category.emotions.Any()) throw new InvalidOperationException("Category not empty");
        _db.Remove(category);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateEmotionCategoryAsync(int id, Models.EmotionCategory patch)
    {
        var category = await _db.EmotionCategories.FindAsync(id);
        if (category == null) throw new KeyNotFoundException("Category not found");
        category.Name = patch.Name;
        category.AllowMultiple = patch.AllowMultiple;
        await _db.SaveChangesAsync();
    }

    public async Task<Models.EmotionCategory> GetEmotionCategoryAsync(int id, bool includeEmotions = true)
    {
        EmotionCategory category;
        if (includeEmotions) category = await _db.EmotionCategories.Include(x => x.emotions).SingleOrDefaultAsync(x => x.Id == id);
        else category = await _db.EmotionCategories.FindAsync(id);
        if (category == null) throw new KeyNotFoundException("Category not found");
        if (!includeEmotions) category.emotions = new List<Emotion>();
        return new Models.EmotionCategory
        {
            AllowMultiple = category.AllowMultiple,
            emotions = category.emotions.Select(x => new Models.Emotion
            {
                Name = x.Name,
                Id = x.Id
            }).ToList(),
            Id = category.Id,
            Name = category.Name
        };
    }

    public async Task<Models.Emotion> GetEmotionAsync(int id)
    {
        var emotion = await _db.Emotions.Include(x => x.Category).SingleOrDefaultAsync(x => x.Id == id);
        if (emotion == null) throw new KeyNotFoundException();
        return new Models.Emotion
        {
            Category = new Models.EmotionCategory
            {
                AllowMultiple = emotion.Category.AllowMultiple,
                emotions = null,
                Id = emotion.Category.Id,
                Name = emotion.Category.Name
            },
            Id = emotion.Id,
            Name = emotion.Name
        };
    }

    public async Task ModifyEmotionAsync(int id, Models.Emotion patch)
    {
        var emotion = await _db.Emotions.Include(x => x.Category).SingleOrDefaultAsync(x => x.Id == id);
        if (emotion == null) throw new KeyNotFoundException();
        emotion.Name = patch.Name;
        emotion.CategoryId = patch.Category.Id;
        await _db.SaveChangesAsync();
    }
} 