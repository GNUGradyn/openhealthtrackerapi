using OpenHealthTrackerApi.Data.Models;
using Activity = OpenHealthTrackerApi.Models.Activity;

namespace OpenHealthTrackerApi.Services.DAL;

public interface IActivityDbService
{
    Task<List<Models.Activity>> GetActivitiesByIdsAsync(int[]? ids);
    Task<List<Activity>> GetActivitiesByUserAsync();
    Task<int> CreateActivity(string name, string icon, int IconType);
    Task DeleteActivity(int id);
    Task ModifyActivity(int id, Models.Activity patch);
}