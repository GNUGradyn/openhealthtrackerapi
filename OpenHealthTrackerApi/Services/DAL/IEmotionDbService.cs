using OpenHealthTrackerApi.Data.Models;
using Emotion = OpenHealthTrackerApi.Models.Emotion;
using EmotionCategory = OpenHealthTrackerApi.Models.EmotionCategory;

namespace OpenHealthTrackerApi.Services.DAL;

public interface IEmotionDbService
{
    Task<List<Emotion>> GetEmotionsByIdsAsync(int[]? ids);
    Task<List<Models.Emotion>> GetEmotionsByUserAsync();
    Task<List<EmotionCategory>> GetEmotionCategoriesByUserAsync(bool includeEmotions = true);
    Task<int> CreateEmotionAsync(string name, int categoryId);
    Task DeleteEmotionAsync(int id);
    Task DeleteEmotionCategoryAsync(int id);
    Task<int> CreateEmotionCategoryAsync(string name, bool allowMultiple = false);
    Task UpdateEmotionCategoryAsync(int id, Models.EmotionCategory patch);
    Task<Models.EmotionCategory> GetEmotionCategoryAsync(int id, bool inclueEmotions = true);
    Task<Models.Emotion> GetEmotionAsync(int id);
    Task ModifyEmotionAsync(int id, Models.Emotion patch);
}