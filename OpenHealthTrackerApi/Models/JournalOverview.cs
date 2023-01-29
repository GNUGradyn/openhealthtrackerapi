using OpenHealthTrackerApi.Data.Models;

namespace OpenHealthTrackerApi.Models;

public class JournalOverview
{
    public List<JournalEntry> Recent { get; set; }
    public List<Emotion> Emotions { get; set; }
    public List<Activity> Activities { get; set; }
}