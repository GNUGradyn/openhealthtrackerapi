using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenHealthTrackerApi.Data.Models;

public class Emotion
{
    [Key]
    [Column("Id")]
    public int Id { get; set; }
    
    [Column("Name")]
    public string Name { get; set; }
    
    [Column("User")]
    public Guid UserId { get; set; }
    
    [Column("EmotionCategoryId")]
    public int CategoryId { get; set; }
    [Column("Icon")]
    public string Icon { get; set; }
    [Column("IconType")]
    public int IconTypeId { get; set; }
    
    // FK relationships
    public List<EmotionEntry> Entries { get; set; }
    public EmotionCategory Category { get; set; }
    public IconType IconType { get; set; }
}