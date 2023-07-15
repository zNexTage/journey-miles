using System;
using System.ComponentModel.DataAnnotations;

namespace API.Models;

public class Deposition
{
    [Key]
    [Required]
    public int Id { get; set;}

    [Required(ErrorMessage = "Informe a descrição")]
    [MaxLength(80)]
    public string Description { get; set;}

    [Required(ErrorMessage = "Informe a foto")]
    [MaxLength(500)]
    public string Photo { get; set; }

    [Required(ErrorMessage = "Informe seu nome")]
    [MaxLength(80)]
    public string PersonName { get; set; }
}