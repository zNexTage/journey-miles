using API.DTO.Deposition;
using API.DTO.Destination;
using API.Models;
using API.Service.Providers;
using API.Utils;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace API;

public class DestinationService : IDestinationService
{
    private AppDbContext _appDbContext;
    private IMapper _mapper;
    private FileManager _fileManager;
    private IUrlHelper _urlHelper;
    private IHttpContextAccessor _httpRequest;

    public DestinationService(
        AppDbContext appDbContext,
        IMapper mapper,
        IWebHostEnvironment environment,
        IUrlHelperFactory urlHelperFactory,
        IActionContextAccessor actionContextAccessor,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
        _fileManager = new FileManager(environment);
        _httpRequest = httpContextAccessor;

        _urlHelper =
            urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
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
        var destination = _mapper.Map<Destination>(destinationDto);

        destination.Photo = _fileManager.SaveFile("destination", photo);

        _appDbContext.Destinations.Add(destination);

        _appDbContext.SaveChanges();

        return _mapper.Map<ReadDestinationDto>(destination);
    }

    public ReadDestinationDto Update(int id, UpdateDestinationDto destinationDto, IFormFile? photo)
    {
        throw new NotImplementedException();
    }
}
