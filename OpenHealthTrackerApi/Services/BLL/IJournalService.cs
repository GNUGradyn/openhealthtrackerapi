using OpenHealthTrackerApi.Data.Models;
using OpenHealthTrackerApi.Models;
using Activity = OpenHealthTrackerApi.Models.Activity;
using Emotion = OpenHealthTrackerApi.Models.Emotion;
using EmotionCategory = OpenHealthTrackerApi.Models.EmotionCategory;
using JournalEntry = OpenHealthTrackerApi.Models.JournalEntry;

namespace OpenHealthTrackerApi.Services.BLL;

public interface IJournalService
{
    Task<List<JournalEntry>> GetEntriesAsync(int count, int start);
    Task<int> CreateEntry(string text, int[]? emotionIds, int[]? activityIds);
    Task<List<Emotion>> GetEmotionsAsync();
    Task<List<Activity>> GetActivitiesAsync();
    Task<List<EmotionCategory>> GetEmotionCategoriesAsync();
    Task<int> CreateEmotionCategoryAsync(string name, bool allowMultiple = false);
    Task<int> CreateEmotionAsync(string name, string icon, int category);
    Task<int> CreateActivityAsync(string name);
    Task DeleteActivityAsync(int id);
    Task DeleteEmotionAsync(int id);
    Task DeleteEmotionCategoryAsync(int id);
    Task DeleteEntryAsync(int id);
    Task<JournalOverview> GetJournalOverviewAsync();
    Task RenameEmotionCategoryAsync(int id, string name);
    Task SetAllowMultipleForCategoryAsync(int id, bool value);
    Task RenameEmotionAsync(int id, string value);
    Task RenameActivityAsync(int id, string value);
    Task MoveEmotionAsync(int id, int newCategory);
}