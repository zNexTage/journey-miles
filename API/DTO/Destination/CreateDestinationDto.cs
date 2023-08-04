using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTO.Destination;

public class CreateDestinationDto
{

    [Required(ErrorMessage = "Informe o nome do destino")]  
    public string Name { get; set; }

    [Required(ErrorMessage = "Informe o preço")]
    [DataType(DataType.Currency)]
    public double Price { get; set; }
}