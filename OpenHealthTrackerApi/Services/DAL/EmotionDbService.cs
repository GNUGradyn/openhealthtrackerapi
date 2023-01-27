using Microsoft.EntityFrameworkCore;
using OpenHealthTrackerApi.Data;
using OpenHealthTrackerApi.Data.Models;

namespace OpenHealthTrackerApi.Services.DAL;

public class EmotionDbService : IEmotionDbService
{
    private readonly DbFactory _dbFactory;

    public EmotionDbService(DbFactory dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task<Emotion[]> GetEmotionsByIdsAsync(int[] ids)
    {
        using (var db = _dbFactory.OHT())
        {
            var emotions = await db.Emotions.Where(x => ids.Contains(x.Id)).ToListAsync();

            if (ids.Any(x => emotions.All(y => x != y.Id))) throw new KeyNotFoundException("Emotion not found");

            return emotions.ToArray();
        }
    }
}