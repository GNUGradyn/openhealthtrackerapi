using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OpenHealthTrackerApi.Data.Models;
using OpenHealthTrackerApi.Models;
using OpenHealthTrackerApi.Pipeline;
using OpenHealthTrackerApi.Services.DAL;
using Activity = OpenHealthTrackerApi.Models.Activity;
using Emotion = OpenHealthTrackerApi.Models.Emotion;
using EmotionCategory = OpenHealthTrackerApi.Models.EmotionCategory;

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
    
    public async Task<JournalOverview> GetJournalOverviewAsync()
    {
        var recent = await GetEntriesAsync(10, 0);
        var activities = await GetActivitiesAsync();
        var emotions = await GetEmotionsAsync();

        return new JournalOverview
        {
            Recent = recent,
            Activities = activities,
            Emotions = emotions
        };
    }

    public async Task<List<Models.JournalEntry>> GetEntriesAsync(int count, int start)
    {
        return await _journalDbService.GetEntriesAsync(count, start);
    }

    public async Task<int> CreateEntry(string text, int[]? emotionIds, int[]? activityIds)
    {
        List<Emotion> emotions;
        // handle emotions
        try
        {
            emotions = await _emotionDbService.GetEmotionsByIdsAsync(emotionIds);
        }
        catch (KeyNotFoundException ex)
        {
            throw new HttpNotFoundExeption("Emotion not found", ex);
        }

        if (emotions.GroupBy(x => x.Category.Id).Any(x => x.Count() > 1 && !x.First().Category.AllowMultiple))
        {
            throw new ArgumentException("Only 1 emotion per category is allowed");
        }

        // Handle activities
        List<Models.Activity> activities;
        try
        {
            activities = await _activityDbService.GetActivitiesByIdsAsync(activityIds);
        }
        catch (KeyNotFoundException ex)
        {
            throw new HttpNotFoundExeption("Activity not found", ex);
        }

        var result = await _journalDbService.CreateEntryAsync(text, emotions, activities);
        return result;
    }

    public async Task<List<Models.Emotion>> GetEmotionsAsync()
    {
        return await _emotionDbService.GetEmotionsByUserAsync();
    }

    public async Task<List<Models.Activity>> GetActivitiesAsync()
    {
        return await _activityDbService.GetActivitiesByUserAsync();
    }

    public async Task<List<Models.EmotionCategory>> GetEmotionCategoriesAsync()
    {
        return await _emotionDbService.GetEmotionCategoriesByUserAsync();
    }

    public async Task<int> CreateEmotionCategoryAsync(string name, bool allowMultiple = false)
    {
        return await _emotionDbService.CreateEmotionCategoryAsync(name, allowMultiple);
    }

    public async Task<int> CreateEmotionAsync(string name, string icon, int category)
    {
        var categories = await GetEmotionCategoriesAsync();
        if (categories.Select(x => x.Id).Contains(category))
        {
            return await _emotionDbService.CreateEmotionAsync(name, icon, category);
        }

        throw new HttpNotFoundExeption("Category not found");
    }

    public async Task<int> CreateActivityAsync(string name)
    {
        return await _activityDbService.CreateActivity(name);
    }

    public async Task DeleteActivityAsync(int id)
    {
        await _activityDbService.DeleteActivity(id);
    }

    public async Task DeleteEmotionAsync(int id)
    {
        await _emotionDbService.DeleteEmotionAsync(id);
    }

    public async Task DeleteEmotionCategoryAsync(int id)
    {
        await _emotionDbService.DeleteEmotionCategoryAsync(id);
    }

    public async Task DeleteEntryAsync(int id)
    {
        await _journalDbService.DeleteEntry(id);
    }

    public async Task RenameEmotionCategoryAsync(int id, string name)
    {
        try
        {
            var category = await _emotionDbService.GetEmotionCategoryAsync(id, false);
            category.Name = name;
            await _emotionDbService.UpdateEmotionCategoryAsync(id, category);
        }
        catch (KeyNotFoundException _)
        {
            throw new HttpNotFoundExeption("Category not found");
        }
    }

    public async Task SetAllowMultipleForCategoryAsync(int id, bool value)
    {
        try
        {
            var category = await _emotionDbService.GetEmotionCategoryAsync(id, false);
            category.AllowMultiple = value;
            await _emotionDbService.UpdateEmotionCategoryAsync(id, category);
        }
        catch (KeyNotFoundException _)
        {
            throw new HttpNotFoundExeption("Category not found");
        }
    }

    public async Task RenameEmotionAsync(int id, string value)
    {
        try
        {
            var emotion = await _emotionDbService.GetEmotionAsync(id);
            emotion.Name = value;
            await _emotionDbService.ModifyEmotionAsync(id, emotion);
        }
        catch (KeyNotFoundException _)
        {
            throw new HttpNotFoundExeption("Emotion not found");
        }
    }

    public async Task RenameActivityAsync(int id, string value)
    {
        var activity = (await _activityDbService.GetActivitiesByIdsAsync(new[] { id })).SingleOrDefault();
        if (activity == null) throw new HttpNotFoundExeption("Activity not found");
        activity.Name = value;
        await _activityDbService.ModifyActivity(id, activity);
    }

    public async Task MoveEmotionAsync(int id, int newCategory)
    {
        var emotion = await _emotionDbService.GetEmotionAsync(id);
        if (emotion == null) throw new HttpNotFoundExeption("Activity not found");
        try
        {
            await _emotionDbService.GetEmotionCategoryAsync(newCategory);
        }
        catch (KeyNotFoundException _)
        {
            throw new HttpNotFoundExeption("Category not found");
        }
        emotion.Category.Id = newCategory;
        await _emotionDbService.ModifyEmotionAsync(id, emotion);
    }
}