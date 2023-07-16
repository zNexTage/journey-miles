using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTO.Deposition;

public class UpdateDepositionDto
{
    [MaxLength(80, ErrorMessage = "A descrição deve ter no máximo 80 caracteres")]
    public string Description { get; set;}

    [MaxLength(80, ErrorMessage = "O nome deve ter no máximo 80 caracteres")]
    public string PersonName { get; set; }
}