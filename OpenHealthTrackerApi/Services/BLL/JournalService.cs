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

    public async Task<JournalEntry[]> GetEntriesAsync(int count, int start, Guid user)
    {
        return await _journalDbService.GetEntriesAsync(count, start, user);
    }
    
    public async Task<int> CreateEntry(string text, int[]? emotionIds, int[]? activityIds, Guid user)
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

        var result = await _journalDbService.CreateEntryAsync(text, emotions, activities, user);
        return result;
    }

    public async Task<Emotion[]> GetEmotionsByUserAsync(Guid user)
    {
        return await _emotionDbService.GetEmotionsByUserAsync(user);
    }

    public async Task<Activity[]> GetActivitiesByUserAsync(Guid user)
    {
        return await _activityDbService.GetActivitiesByUserAsync(user);
    }

    public async Task<EmotionCategory[]> GetEmotionCategoriesByUserAsync(Guid user)
    {
        return await _emotionDbService.GetEmotionCategoriesByUserAsync(user);
    }

    public async Task<int> CreateEmotionCategoryAsync(string name, Guid user)
    {
        return await _emotionDbService.CreateEmotionCategoryAsync(name, user);
    }
}