using OpenHealthTrackerApi.Data.Models;
using OpenHealthTrackerApi.Services.DAL;

namespace OpenHealthTrackerApi.Services.BLL;

public class JournalService : IJournalService
{
    private readonly IActivityDbService _activityDbService;
    private readonly IEmotionDbService _emotionDbService;
    private readonly IJournalDbService _journalDbService;

    public JournalService(IActivityDbService activityDbService, IEmotionDbService emotionDbService,
        IJournalDbService journalDbService)
    {
        _activityDbService = activityDbService;
        _emotionDbService = emotionDbService;
        _journalDbService = journalDbService;
    }

    public async Task<List<Models.JournalEntry>> GetEntriesAsync(int count, int start)
    {
        return await _journalDbService.GetEntriesAsync(count, start);
    }
    
    public async Task<int> CreateEntry(string text, int[]? emotionIds, int[]? activityIds)
    {
        Emotion[] emotions;
        // handle emotions
        try
        {
            emotions = await _emotionDbService.GetEmotionsByIdsAsync(emotionIds);
        }
        catch (KeyNotFoundException ex)
        {
            throw new KeyNotFoundException("Emotion not found", ex);
        }

        if (emotions.GroupBy(x => x.CategoryId).Any(x => x.Count() > 1))
        {
            throw new ArgumentException("Only 1 emotion per category is allowed");
        }

        // Handle activities
        Activity[] activities;
        try
        {
            activities = await _activityDbService.GetActivitiesByIdsAsync(activityIds);
        }
        catch (KeyNotFoundException ex)
        {
            throw new KeyNotFoundException("Activity not found", ex);
        }

        var result = await _journalDbService.CreateEntryAsync(text, emotions, activities);
        return result;
    }

    public async Task<Emotion[]> GetEmotionsAsync()
    {
        return await _emotionDbService.GetEmotionsByUserAsync();
    }

    public async Task<Activity[]> GetActivitiesAsync()
    {
        return await _activityDbService.GetActivitiesByUserAsync();
    }

    public async Task<List<Models.EmotionCategory>> GetEmotionCategoriesAsync()
    {
        return await _emotionDbService.GetEmotionCategoriesByUserAsync();
    }

    public async Task<int> CreateEmotionCategoryAsync(string name)
    {
        return await _emotionDbService.CreateEmotionCategoryAsync(name);
    }

    public async Task<int> CreateEmotionAsync(string name, int category)
    {
        var categories = await GetEmotionCategoriesAsync();
        if (categories.Select(x => x.Id).Contains(category))
        {
            return await _emotionDbService.CreateEmotionAsync(name, category);
        }
        throw new KeyNotFoundException("Category not found");
    }

    public async Task<int> CreateActivityAsync(string name)
    {
        return await _activityDbService.CreateActivity(name);
    }

    public async Task DeleteActivityAsync(int id)
    {
        await _activityDbService.DeleteActivity(id);
    }
}