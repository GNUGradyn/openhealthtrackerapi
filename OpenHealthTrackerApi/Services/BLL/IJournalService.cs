namespace OpenHealthTrackerApi.Services.BLL;

public interface IJournalService
{
    Task CreateEntry(string text, int[]? emotionIds, int[]? activityIds, Guid user);
}