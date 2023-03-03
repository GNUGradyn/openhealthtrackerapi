namespace OpenHealthTrackerApi.Models;

public class Emotion
{
    public int Id { get; set; }
    public string Name { get; set; }
    public EmotionCategory Category { get; set; }
    public string Icon { get; set; }
    public bool IsCustom
    {
        get { return Icon.StartsWith("/"); }
    }
    
    public string IconType { get; set; }
}