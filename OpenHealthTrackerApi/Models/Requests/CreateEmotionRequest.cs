namespace OpenHealthTrackerApi.Models;

public class CreateEmotionRequest
{
    public string Name { get; set; }
    public string Icon { get; set; }
    public int Category { get; set; }
}