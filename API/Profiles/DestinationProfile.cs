using API.DTO.Destination;
using API.Models;
using AutoMapper;

namespace API;

public class DestinationProfile : Profile
{
    public DestinationProfile()
    {
        CreateMap<Destination, ReadDestinationDto>();
        CreateMap<CreateDestinationDto, Destination>();
    }
}
