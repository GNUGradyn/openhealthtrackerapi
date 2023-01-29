using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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

    public async Task<List<Models.Activity>> GetActivitiesByIdsAsync(int[]? ids) 
    {
        if (ids == null) return new List<Models.Activity>();
        var activities = await _db.Activities.Where(x => ids.Contains(x.Id)).ToListAsync();

        if (ids.Any(x => activities.All(y => x != y.Id))) throw new KeyNotFoundException("Activity not found");

        return activities.Select(x => new Models.Activity
        {
            Id = x.Id,
            Name = x.Name
        }).ToList();
    }

    public async Task<List<Models.Activity>> GetActivitiesByUserAsync() 
    {
        var activities = _db.Activities.Where(x => x.User == _user);

        return await activities.Select(x => new Models.Activity
        {
            Id = x.Id,
            Name = x.Name
        }).ToListAsync();
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