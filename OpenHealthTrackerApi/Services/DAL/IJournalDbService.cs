using OpenHealthTrackerApi.Data.Models;
using JournalEntry = OpenHealthTrackerApi.Models.JournalEntry;

namespace OpenHealthTrackerApi.Services.DAL;

public interface IJournalDbService
{
    Task<int> CreateEntryAsync(string text, Emotion[] emotions, Activity[] activities);
    Task<List<JournalEntry>> GetEntriesAsync(int count, int start);
}