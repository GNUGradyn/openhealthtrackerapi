namespace OpenHealthTrackerApi.Models;

public class JournalEntry
{
    public int Id { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<Emotion> Emotions { get; set; } = new();
    public List<Activity> Activities { get; set; } = new();
}