using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenHealthTrackerApi.Data.Models;

[Table("User")]
public class User
{
    [Key]
    [Column("Id")]
    public int Id { get; set; }
    
    public List<JournalEntry> JournalEntries { get; set; }
}