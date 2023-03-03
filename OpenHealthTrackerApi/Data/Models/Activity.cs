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
    
    [Column("Icon")]
    public string Icon { get; set; }
    [Column("IconType")]
    public int IconTypeId { get; set; }
    
    // FK relationships
    public List<ActivityEntry> Entries { get; set; }
    public IconType IconType { get; set; }
}