using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenHealthTrackerApi.Data.Models;

[Table("ActivityEntry")]
public class ActivityEntry
{
    [Key]
    [Column("Id")]
    public int Id { get; set; }
    
    [Column("ActivityId")]
    public int ActivityId { get; set; }
    
    [Column("JournalEntryId")]
    public int JournalEntryId { get; set; }
    
    // FK relationships
    public JournalEntry JournalEntry { get; set; }
    public Activity Activity { get; set; }
}