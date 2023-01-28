namespace OpenHealthTrackerApi.Models;

public class EmotionCategoryRequest
{
    public EmotionCategoryRequest(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
}