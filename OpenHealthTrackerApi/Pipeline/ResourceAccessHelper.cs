using Microsoft.EntityFrameworkCore;
using OpenHealthTrackerApi.Data;

namespace OpenHealthTrackerApi.Pipeline;

public class ResourceAccessHelper : IResourceAccessHelper
{
    private readonly DbFactory _dbFactory;

    public ResourceAccessHelper(DbFactory dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task<bool> ValidateActivityAccess(int id, Guid user)
    {
        using (var db = _dbFactory.OHT())
        {
            return await db.Activities.AnyAsync(x => x.Id == id && x.User == user);
        }
    }
}