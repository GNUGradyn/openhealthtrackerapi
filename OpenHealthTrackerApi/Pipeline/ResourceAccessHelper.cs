using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using OpenHealthTrackerApi.Data;

namespace OpenHealthTrackerApi.Pipeline;

public class ResourceAccessHelper : IResourceAccessHelper
{
    private readonly OHTDbContext _db;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly Guid _user;

    public ResourceAccessHelper(OHTDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        _db = dbContext;
        _httpContextAccessor = httpContextAccessor;
        _user = new Guid(_httpContextAccessor.HttpContext.User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);
    }

    public async Task ValidateActivityAccess(params int[] ids)
    {
        if (ids == null) ids = Array.Empty<int>();
        foreach (var id in ids)
        {
            if (!await _db.Activities.AnyAsync(x => x.Id == id && x.User == _user))
            {
                throw new AccessDeniedException();
            }
        }
    }

    public async Task ValidateEmotionAccess(params int[] ids)
    {
        if (ids == null) ids = Array.Empty<int>();
        foreach (var id in ids)
        {
            if (!await _db.Emotions.AnyAsync(x => x.Id == id && x.UserId == _user))
            {
                throw new AccessDeniedException();
            }
        }
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