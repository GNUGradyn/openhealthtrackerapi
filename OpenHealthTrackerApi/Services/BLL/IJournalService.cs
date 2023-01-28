using OpenHealthTrackerApi.Data.Models;

namespace OpenHealthTrackerApi.Services.BLL;

public interface IJournalService
{
    Task<int> CreateEntry(string text, int[]? emotionIds, int[]? activityIds, Guid user);
    Task<Emotion[]> GetEmotionsByUserAsync(Guid user);
    Task<Activity[]> GetActivitiesByUserAsync(Guid user);
    Task<EmotionCategory[]> GetEmotionCategoriesByUserAsync(Guid user);
    Task<JournalEntry[]> GetEntriesAsync(int count, int start, Guid user);
    Task<int> CreateEmotionCategoryAsync(string name, Guid user);
}