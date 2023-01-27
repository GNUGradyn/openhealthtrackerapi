using OpenHealthTrackerApi.Data.Models;

namespace OpenHealthTrackerApi.Services.DAL;

public interface IEmotionDbService
{
    Task<Emotion[]> GetEmotionsByIdsAsync(int[]? ids);
}