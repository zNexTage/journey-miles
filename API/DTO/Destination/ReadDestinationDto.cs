using System;

namespace API.DTO.Destination;

public class ReadDestinationDto
{
    public int Id { get; set; }
 
    public string Name { get; set; }
    public double Price { get; set; }
    public List<string> Photos { get; set; } = new List<string>();
    public string DescritiveText { get; set; }
}