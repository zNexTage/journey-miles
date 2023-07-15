using System;
using API.DTO.Deposition;
using API.Models;
using AutoMapper;

namespace API.Profiles;

public class DepositionProfile : Profile
{
    public DepositionProfile()
    {
        CreateMap<Deposition, ReadDepositionDto>();

        CreateMap<CreateDepositionDto, Deposition>()
        .ForMember(dto => dto.Photo, opt => opt.Ignore());
    }
}
