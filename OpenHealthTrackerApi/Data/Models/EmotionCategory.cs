using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenHealthTrackerApi.Data.Models;

[Table("EmotionCategory")]
public class EmotionCategory
{
    [Key]
    [Column("Id")]
    public int Id { get; set; }
    
    [Column("User")]
    public Guid User { get; set; }
    
    [Column("Name")]
    public string Name { get; set; }
    
    [Column("AllowMultiple")]
    public bool AllowMultiple { get; set; }
    
    // FK relationships
    public List<Emotion> emotions { get; set; }
}