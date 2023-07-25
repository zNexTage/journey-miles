using API.DTO.Destination;
using API.Service.Providers;
using AutoMapper;

namespace API;

public class DestinationService : IDestinationService
{
    private AppDbContext _appDbContext;
    private IMapper _mapper;

    public DestinationService(AppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;   
    }
    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<ReadDestinationDto> GetAll()
    {
        var destinations = _appDbContext.Destinations.ToList();

        return _mapper.Map<List<ReadDestinationDto>>(destinations);
    }

    public ReadDestinationDto GetById(int id)
    {
        throw new NotImplementedException();
    }

    public ReadDestinationDto Register(CreateDestinationDto destinationDto, IFormFile photo)
    {
        throw new NotImplementedException();
    }

    public ReadDestinationDto Update(int id, UpdateDestinationDto destinationDto, IFormFile? photo)
    {
        throw new NotImplementedException();
    }
}
