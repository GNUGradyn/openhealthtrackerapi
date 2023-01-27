using Microsoft.EntityFrameworkCore;
using OpenHealthTrackerApi.Data;
using OpenHealthTrackerApi.Data.Models;

namespace OpenHealthTrackerApi.Services.DAL;

public class ActivityDbService : IActivityDbService
{
    private readonly DbFactory _dbFactory;

    public ActivityDbService(DbFactory dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task<Activity[]> GetActivitiesByIdsAsync(int[]? ids)
    {
        if (ids == null) return Array.Empty<Activity>();
        using (var db = _dbFactory.OHT())
        {
            var activities = await db.Activities.Where(x => ids.Contains(x.Id)).ToListAsync();

            if (ids.Any(x => activities.All(y => x != y.Id))) throw new KeyNotFoundException("Activity not found");

            return activities.ToArray();
        }
    }
}