using OpenHealthTrackerApi.Services.DAL;

namespace OpenHealthTrackerApi.Services.BLL;

public class JournalService
{
    private readonly IActivityDbService _activityDbService;
    private readonly IEmotionDbService _emotionDbService;

    public JournalService(IActivityDbService activityDbService, IEmotionDbService emotionDbService)
    {
        _activityDbService = activityDbService;
        _emotionDbService = emotionDbService;
    }

    public async Task<int> CreateEntry(string text, int[] emotions, int[] activities)
    {
        
    }
}