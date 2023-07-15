using System;

namespace API.DTO.Deposition;

public class ReadDepositionDto
{
    public int Id { get; set; }
    public string Description { get; set;}

    public string Photo { get; set; }
    public string PersonName { get; set; }
}