using OpenHealthTrackerApi.Data.Models;
using Emotion = OpenHealthTrackerApi.Models.Emotion;
using EmotionCategory = OpenHealthTrackerApi.Models.EmotionCategory;
using JournalEntry = OpenHealthTrackerApi.Models.JournalEntry;

namespace OpenHealthTrackerApi.Services.BLL;

public interface IJournalService
{
    Task<List<JournalEntry>> GetEntriesAsync(int count, int start);
    Task<int> CreateEntry(string text, int[]? emotionIds, int[]? activityIds);
    Task<List<Emotion>> GetEmotionsAsync();
    Task<Activity[]> GetActivitiesAsync();
    Task<List<EmotionCategory>> GetEmotionCategoriesAsync();
    Task<int> CreateEmotionCategoryAsync(string name);
    Task<int> CreateEmotionAsync(string name, int category);
    Task<int> CreateActivityAsync(string name);
    Task DeleteActivityAsync(int id);
}