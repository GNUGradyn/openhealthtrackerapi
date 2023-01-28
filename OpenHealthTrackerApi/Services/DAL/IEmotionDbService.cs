using OpenHealthTrackerApi.Data.Models;

namespace OpenHealthTrackerApi.Services.DAL;

public interface IEmotionDbService
{
    Task<Emotion[]> GetEmotionsByIdsAsync(int[]? ids);
    Task<Emotion[]> GetEmotionsByUserAsync();
    Task<EmotionCategory[]> GetEmotionCategoriesByUserAsync();
    Task<int> CreateEmotionCategoryAsync(string name);
    Task<int> CreateEmotionAsync(string name, int categoryId);
}