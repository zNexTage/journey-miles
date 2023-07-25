using API.Controllers;
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
    private const string BASE_PHOTO_PATH = "destination";

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

    /// <summary>
    /// Retorna a URL da foto (endpoint) de um destino.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private string GetDestinationPhotoEndpointUrl(int id)
    {
        var controllerName = nameof(DestinationController).Replace("Controller", string.Empty);
        var methodName = "GetPhoto";

        var url = _urlHelper.Action(
                methodName,
                controllerName,
                new { id },
                _httpRequest.HttpContext.Request.Scheme
        );

        return url;
    }

    public IEnumerable<ReadDestinationDto> GetAll()
    {
        var destinations = _appDbContext.Destinations.ToList();

        var depositionsDto = _mapper.Map<List<ReadDestinationDto>>(destinations);

        depositionsDto.ForEach(dto =>
        {
            // we will use the endpoint to serve the photo
            dto.Photo = this.GetDestinationPhotoEndpointUrl(dto.Id);
        });

        return depositionsDto;
    }

    public ReadDestinationDto GetById(int id)
    {
        var destination = _appDbContext.Destinations.FirstOrDefault(destination => destination.Id == id)
        ?? throw new Destination.DoesNotExists($"Destino {id} não foi localizado");

        var destinationDto = _mapper.Map<ReadDestinationDto>(destination);

        destinationDto.Photo = this.GetDestinationPhotoEndpointUrl(id);

        return destinationDto;
    }

    /// <summary>
    /// Obtém o caminho da foto no diretório
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="Destination.DoesNotExists"></exception>
    public string GetPhotoDirectory(int id)
    {
        var destination = _appDbContext.Destinations.FirstOrDefault(destination => destination.Id == id)
        ?? throw new Destination.DoesNotExists($"Destino {id} não foi localizado");

        return destination.Photo;
    }

    public ReadDestinationDto Register(CreateDestinationDto destinationDto, IFormFile photo)
    {
        var destination = _mapper.Map<Destination>(destinationDto);

        destination.Photo = _fileManager.SaveFile(BASE_PHOTO_PATH, photo);

        _appDbContext.Destinations.Add(destination);

        _appDbContext.SaveChanges();

        return _mapper.Map<ReadDestinationDto>(destination);
    }

    public ReadDestinationDto Update(int id, UpdateDestinationDto destinationDto, IFormFile? photo)
    {
        var destination = _appDbContext.Destinations.FirstOrDefault(destination => destination.Id == id)
       ?? throw new Destination.DoesNotExists($"Destino {id} não foi localizado");

        _mapper.Map(destinationDto, destination);

        if (photo != null)
        {
            //Remove the old photo
            _fileManager.Remove(destination.Photo);

            //Save the new photo
            destination.Photo = _fileManager.SaveFile(BASE_PHOTO_PATH, photo);
        }

        _appDbContext.SaveChanges();

        var readDestDto = _mapper.Map<ReadDestinationDto>(destination);
        readDestDto.Photo = this.GetDestinationPhotoEndpointUrl(destination.Id);

        return readDestDto;
    }

    public void Delete(int id)
    {
        var destination = _appDbContext.Destinations.FirstOrDefault(dest => dest.Id == id) ??
        throw new Destination.DoesNotExists($"Destino {id} não localizado");

        _appDbContext.Destinations.Remove(destination);
        //Remove the destination photo
        _fileManager.Remove(destination.Photo);

        _appDbContext.SaveChanges();
    }
}
