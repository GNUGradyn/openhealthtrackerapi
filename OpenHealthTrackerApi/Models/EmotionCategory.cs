using OpenHealthTrackerApi.Data.Models;

namespace OpenHealthTrackerApi.Models;

public class EmotionCategory
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool AllowMultiple { get; set; }
    public List<Emotion> emotions { get; set; }
}