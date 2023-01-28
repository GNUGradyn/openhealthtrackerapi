using OpenHealthTrackerApi.Data.Models;

namespace OpenHealthTrackerApi.Services.DAL;

public interface IEmotionDbService
{
    Task<Emotion[]> GetEmotionsByIdsAsync(int[]? ids);
    Task<Emotion[]> GetEmotionsByUserAsync(Guid user);
    Task<EmotionCategory[]> GetEmotionCategoriesByUserAsync(Guid user);
}