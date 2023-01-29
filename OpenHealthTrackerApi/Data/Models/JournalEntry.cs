using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenHealthTrackerApi.Data.Models;

[Table("JournalEntry")]
public class JournalEntry
{
    [Key] 
    [Column("Id")] 
    public int Id { get; set; }
    
    [Column("User")]
    public Guid UserId { get; set; }
    
    [Column("Body")]
    public string Body { get; set; }
    
    [Column("CreatedAt")]
    public DateTime CreatedAt { get; set; }
    
    // FK relationships
    public List<ActivityEntry> Activities { get; set; }
    public List<EmotionEntry> Emotions { get; set; }
}