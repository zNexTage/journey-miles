using System.ComponentModel.DataAnnotations;
using API.Models;

namespace API;

public class Photos
{
    [Required]
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(1000)]
    public string Photo { get; set; }

    [Required]
    public int DestinationId { get; set; }
    
    
    public virtual Destination Destination { get; set; }
}
