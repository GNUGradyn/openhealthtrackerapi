namespace OpenHealthTrackerApi.Models;

public class PatchEmotionCategoryRequest
{
    public int Id { get; set; }
    public string? Name { get; set; } = null;
    public bool? AllowMultiple { get; set; } = null;
}