using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTO.Deposition;

public class CreateDepositionDto
{
    [Required(ErrorMessage = "Informe a descrição")]
    [MaxLength(80, ErrorMessage = "A descrição deve ter no máximo 80 caracteres")]
    public string Description { get; set;}

    // [Required(ErrorMessage = "Informe uma foto")]
    // public byte[] Photo { get; set; }

    [Required(ErrorMessage = "Informe seu nome")]
    [MaxLength(80, ErrorMessage = "O nome deve ter no máximo 80 caracteres")]
    public string PersonName { get; set; }
}
