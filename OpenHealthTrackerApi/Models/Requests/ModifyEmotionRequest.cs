namespace OpenHealthTrackerApi.Models;

public class ModifyEmotionRequest
{
    public int Id { get; set; }
    public string? Name { get; set; } = null;
    public int? Category { get; set; } = null;
}