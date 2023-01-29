using OpenHealthTrackerApi.Data.Models;
using Emotion = OpenHealthTrackerApi.Models.Emotion;
using EmotionCategory = OpenHealthTrackerApi.Models.EmotionCategory;

namespace OpenHealthTrackerApi.Services.DAL;

public interface IEmotionDbService
{
    Task<List<Emotion>> GetEmotionsByIdsAsync(int[]? ids);
    Task<List<Models.Emotion>> GetEmotionsByUserAsync();
    Task<List<EmotionCategory>> GetEmotionCategoriesByUserAsync(bool includeEmotions = true);
    Task<int> CreateEmotionCategoryAsync(string name);
    Task<int> CreateEmotionAsync(string name, int categoryId);
    Task DeleteEmotionAsync(int id);
    Task DeleteEmotionCategoryAsync(int id);
}