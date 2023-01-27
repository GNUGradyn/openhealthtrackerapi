using OpenHealthTrackerApi.Data.Models;

namespace OpenHealthTrackerApi.Services.DAL;

public interface IJournalDbService
{
    Task CreateEntryAsync(string text, Emotion[] emotions, Activity[] activities, Guid user);
}