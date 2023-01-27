using OpenHealthTrackerApi.Data.Models;

namespace OpenHealthTrackerApi.Services.DAL;

public interface IActivityDbService
{
    Task<Activity[]> GetActivitiesByIdsAsync(int[]? ids);
}