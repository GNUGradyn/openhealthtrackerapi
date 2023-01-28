namespace OpenHealthTrackerApi.Pipeline;

public interface IResourceAccessHelper
{
    Task<bool> ValidateActivityAccess(int id, Guid user);
}