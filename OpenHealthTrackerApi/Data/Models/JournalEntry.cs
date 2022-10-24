using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenHealthTrackerApi.Data.Models;

[Table("JournalEntry")]
public class JournalEntry
{
    [Key] 
    [Column("Id")] 
    public int Id { get; set; }
    
    [Column("UserId")]
    public int UserId { get; set; }
    
    [Column("Body")]
    public string Body { get; set; }
    
    [Column("CreatedAt")]
    public DateTime CreatedAt { get; set; }
    
    public User User { get; set; }
    public List<Emotion> Emotions { get; set; }
    public List<Activity> Activities { get; set; }
}