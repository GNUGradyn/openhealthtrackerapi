﻿using OpenHealthTrackerApi.Data.Models;
using OpenHealthTrackerApi.Services.DAL;

namespace OpenHealthTrackerApi.Services.BLL;

public class JournalService : IJournalService
{
    private readonly IActivityDbService _activityDbService;
    private readonly IEmotionDbService _emotionDbService;
    private readonly IJournalDbService _journalDbService;

    public JournalService(IActivityDbService activityDbService, IEmotionDbService emotionDbService, IJournalDbService journalDbService)
    {
        _activityDbService = activityDbService;
        _emotionDbService = emotionDbService;
        _journalDbService = journalDbService;
    }

    public async Task CreateEntry(string text, int[]? emotionIds, int[]? activityIds, Guid user)
    {
        Emotion[] emotions;
        // handle emotions
        try
        {
            emotions = await _emotionDbService.GetEmotionsByIdsAsync(emotionIds);
        }
        catch (KeyNotFoundException ex)
        {
            throw new KeyNotFoundException("Emotion not found", ex);
        }

        if (emotions.GroupBy(x => x.CategoryId).Any(x => x.Count() > 1))
        {
            throw new ArgumentException("Only 1 emotion per category is allowed");
        }

        // Handle activities
        Activity[] activities;
        try
        {
            activities = await _activityDbService.GetActivitiesByIdsAsync(activityIds);
        }
        catch (KeyNotFoundException ex)
        {
            throw new KeyNotFoundException("Activity not found", ex);
        }

        await _journalDbService.CreateEntryAsync(text, emotions, activities, user);
    }
}