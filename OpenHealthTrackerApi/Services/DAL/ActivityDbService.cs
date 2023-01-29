using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using OpenHealthTrackerApi.Data;
using OpenHealthTrackerApi.Data.Models;

namespace OpenHealthTrackerApi.Services.DAL;

public class ActivityDbService : IActivityDbService
{
    private readonly OHTDbContext _db;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly Guid _user;

    public ActivityDbService(OHTDbContext db, IHttpContextAccessor httpContextAccessor)
    {
        _db = db;
        _httpContextAccessor = httpContextAccessor;
        _user = new Guid(_httpContextAccessor.HttpContext.User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);
    }

    public async Task<Activity[]> GetActivitiesByIdsAsync(int[]? ids) // TODO: replace return type with list instead of array, use a DTO
    {
        if (ids == null) return Array.Empty<Activity>();
        var activities = await _db.Activities.Where(x => ids.Contains(x.Id)).ToListAsync();

        if (ids.Any(x => activities.All(y => x != y.Id))) throw new KeyNotFoundException("Activity not found");

        return activities.ToArray();
    }

    public async Task<Activity[]> GetActivitiesByUserAsync() // TODO: replace return type with list instead of array, use a DTO
    {
        return await _db.Activities.Where(x => x.User == _user).ToArrayAsync();
    }

    public async Task<int> CreateActivity(string name)
    {
        var activity = new Activity
        {
            Name = name,
            User = _user
        };
        await _db.Activities.AddAsync(activity);
        await _db.SaveChangesAsync();
        return activity.Id;
    }

    public async Task DeleteActivity(int id)
    {
        var activity = await _db.Activities.FindAsync(id);
        if (activity == null)
        {
            throw new KeyNotFoundException("Activity not found");
        }

        _db.Remove(activity);
        await _db.SaveChangesAsync();
    }
}