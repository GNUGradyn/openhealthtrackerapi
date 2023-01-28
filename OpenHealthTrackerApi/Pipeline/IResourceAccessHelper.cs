namespace OpenHealthTrackerApi.Pipeline;

public interface IResourceAccessHelper
{
    Task ValidateActivityAccess(int id, Guid user);
    Task ValidateEmotionAccess(int id, Guid user);
}