using Microsoft.EntityFrameworkCore;
using OpenHealthTrackerApi.Data;

namespace OpenHealthTrackerApi.Pipeline;

public class ResourceAccessHelper : IResourceAccessHelper
{
    private readonly OHTDbContext _db;

    public ResourceAccessHelper(OHTDbContext dbContext)
    {
        _db = dbContext;
    }

    public async Task ValidateActivityAccess(int id, Guid user)
    {
        if (!await _db.Activities.AnyAsync(x => x.Id == id && x.User == user))
            throw new AccessDeniedException();
    }

    public async Task ValidateEmotionAccess(int id, Guid user)
    {
        if (!await _db.Emotions.AnyAsync(x => x.Id == id && x.UserId == user))
            throw new AccessDeniedException();
    }
}

public class AccessDeniedException : Exception
{
    public AccessDeniedException()
    {
    }

    public AccessDeniedException(string message)
        : base(message)
    {
    }

    public AccessDeniedException(string message, Exception inner)
        : base(message, inner)
    {
    }
}