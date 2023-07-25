using System;

namespace API.DTO.Destination;

public class ReadDestinationDto
{
    public int Id { get; set; }
 
    public string Name { get; set; }
    public double Price { get; set; }
    public string Photo { get; set; }
}