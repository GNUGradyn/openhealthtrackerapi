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
        if (ids == null) return;
        foreach (var id in ids)
        {
            if (!await _db.Activities.AnyAsync(x => x.Id == id && x.User == _user))
            {
                throw new HttpNotFoundExeption("Activity not found");
            }
        }
    }

    public async Task ValidateEmotionAccess(params int[] ids)
    {
        if (ids == null) return;
        foreach (var id in ids)
        {
            if (!await _db.Emotions.AnyAsync(x => x.Id == id && x.UserId == _user))
            {
                throw new HttpNotFoundExeption("Emotion not found");
            }
        }
    }

    public async Task ValidateEmotionCategoryAccess(params int[] ids)
    {
        if (ids == null) return;
        foreach (var id in ids)
        {
            if (!await _db.EmotionCategories.AnyAsync(x => x.Id == id && x.User == _user))
            {
                throw new HttpNotFoundExeption("Emotion Category not found");
            }
        }
    }

    public async Task ValidateJournalEntryAccess(params int[] ids)
    {
        if (ids == null) return;
        foreach (var id in ids)
        {
            if (!await _db.EmotionCategories.AnyAsync(x => x.Id == id && x.User == _user))
            {
                throw new HttpNotFoundExeption("Entry not found");
            }
        }
    }
}