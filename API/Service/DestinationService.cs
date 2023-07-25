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
        var destination = _appDbContext.Destinations.FirstOrDefault(destination => destination.Id == id)
        ?? throw new Destination.DoesNotExists($"Depoimento {id} não foi localizado");

        destination.Photo = this.GetDestinationPhotoEndpointUrl(id);

        return _mapper.Map<ReadDestinationDto>(destination);
    }    

    /// <summary>
    /// Obtém o caminho da foto no diretório
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="Destination.DoesNotExists"></exception>
    public string GetPhotoDirectory(int id){
        var destination = _appDbContext.Destinations.FirstOrDefault(destination => destination.Id == id)
        ?? throw new Destination.DoesNotExists($"Depoimento {id} não foi localizado");

        return destination.Photo;
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
