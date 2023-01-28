using OpenHealthTrackerApi.Data.Models;

namespace OpenHealthTrackerApi.Services.DAL;

public interface IActivityDbService
{
    Task<Activity[]> GetActivitiesByIdsAsync(int[]? ids);
    Task<Activity[]> GetActivitiesByUserAsync(Guid user);
    Task<int> CreateActivity(string name, Guid user);
}