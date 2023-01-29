namespace OpenHealthTrackerApi.Pipeline;

public interface IResourceAccessHelper
{
    Task ValidateActivityAccess(params int[] id);
    Task ValidateEmotionAccess(params int[] id);
    Task ValidateEmotionCategoryAccess(params int[] ids);
}