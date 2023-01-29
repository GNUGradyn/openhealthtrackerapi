using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenHealthTrackerApi.Data.Models;

public class Activity
{
    [Key]
    [Column("Id")]
    public int Id { get; set; }
    
    [Column("Name")]
    public string Name { get; set; }
    
    [Column("User")]
    public Guid User { get; set; }
    
    // FK relationships
    private List<ActivityEntry> Entries { get; set; }
}