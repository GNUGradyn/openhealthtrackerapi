namespace OpenHealthTrackerApi.Models;

public class JournalEntryRequest
{
    public string Text { get; set; }
    public int[]? Emotions { get; set; }
    public int[]? Activities { get; set; }
}