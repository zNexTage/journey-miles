using System;
using System.ComponentModel.DataAnnotations;

namespace API.Models;

public class Destination : BaseModel
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = "Informe o nome do destino")]  
    public string Name { get; set; }

    [Required(ErrorMessage = "Informe o preço")]
    [DataType(DataType.Currency)]
    public double Price { get; set; }

    [Required(ErrorMessage = "Informe a foto do destino")]
    [MaxLength(500)]
    public string Photo { get; set; }    

    [Required(ErrorMessage = "Informe o campo Meta")]
    [MaxLength(160)]
    public string Meta { get; set; }

    [MaxLength(100)]
    public string DescritiveText { get; set; }

    public virtual List<Photos> Photos { get; set; }

}