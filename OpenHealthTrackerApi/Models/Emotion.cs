namespace OpenHealthTrackerApi.Models;

public class Emotion
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool AllowMultiple { get; set; }
    public EmotionCategory Category { get; set; }
}