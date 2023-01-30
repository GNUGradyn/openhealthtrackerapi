namespace OpenHealthTrackerApi.Models;

public class CreateEmotionCategoryRequest
{
    public string Name { get; set; }
    public bool AllowMultiple { get; set; } = false;
}