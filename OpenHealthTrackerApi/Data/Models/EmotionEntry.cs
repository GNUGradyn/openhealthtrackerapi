using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenHealthTrackerApi.Data.Models;

[Table("EmotionEntry")]
public class EmotionEntry
{
    [Key]
    [Column("Id")]
    public int Id { get; set; }
    
    [Column("EmotionId")]
    public int EmotionId { get; set; }
    
    [Column("JournalEntryId")]
    public int JournalEntryId { get; set; }

    // FK relationships
    public JournalEntry JournalEntry { get; set; }
    public Emotion Emotion { get; set; }
    
}