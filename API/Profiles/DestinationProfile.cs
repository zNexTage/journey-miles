using API.DTO.Destination;
using API.Models;
using AutoMapper;

namespace API;

public class DestinationProfile : Profile
{
    public DestinationProfile()
    {
        CreateMap<Destination, ReadDestinationDto>()
        .ForMember(dest => dest.Photos, opt => opt.Ignore());
        
        CreateMap<CreateDestinationDto, Destination>();
        CreateMap<UpdateDestinationDto, Destination>();
    }
}
