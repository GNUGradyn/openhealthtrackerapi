using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenHealthTrackerApi.Data.Models;

[Table("IconType")]
public class IconType
{
    [Key]
    [Column("Id")]
    public int Id { get; set; }
    
    [Column("Name")]
    public string Name { get; set; }
    
    [Column("CDN")]
    public string CDN { get; set; }
}