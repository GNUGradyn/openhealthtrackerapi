﻿using OpenHealthTrackerApi.Data.Models;

namespace OpenHealthTrackerApi.Services.DAL;

public interface IJournalDbService
{
    Task<int> CreateEntryAsync(string text, Emotion[] emotions, Activity[] activities, Guid user);
    Task<JournalEntry[]> GetEntriesAsync(int count, int start, Guid user);
}