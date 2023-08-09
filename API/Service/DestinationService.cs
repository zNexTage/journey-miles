using API.Controllers;
using API.DTO.Destination;
using API.ExternalServices;
using API.Models;
using API.Service.Providers;
using API.Utils;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;

namespace API;

public class DestinationService : IDestinationService
{
    private AppDbContext _appDbContext;
    private IMapper _mapper;
    private FileManager _fileManager;
    private IUrlHelper _urlHelper;
    private IHttpContextAccessor _httpRequest;
    private const string BASE_PHOTO_PATH = "destination";
    private ChatBotService _openAIService;

    public DestinationService(
        AppDbContext appDbContext,
        IMapper mapper,
        IWebHostEnvironment environment,
        IUrlHelperFactory urlHelperFactory,
        IActionContextAccessor actionContextAccessor,
        IHttpContextAccessor httpContextAccessor,
        ChatBotService openAIService
    )
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
        _fileManager = new FileManager(environment);
        _httpRequest = httpContextAccessor;
        _openAIService = openAIService;

        _urlHelper =
            urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
    }

    /// <summary>
    /// Retorna a URL da foto (endpoint) de um destino.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private string GetDestinationPhotoEndpointUrl(int destinationId, int photoId)
    {
        var controllerName = nameof(DestinationController).Replace("Controller", string.Empty);
        var methodName = "GetPhoto";

        var url = _urlHelper.Action(
                methodName,
                controllerName,
                new { destinationId, photoId },
                _httpRequest.HttpContext.Request.Scheme
        );

        return url;
    }

    private IEnumerable<ReadDestinationDto> DestinationsToDto(IEnumerable<Destination> destinations)
    {
        List<ReadDestinationDto> destinationDtos = new();

        foreach (var dest in destinations)
        {
            var destDto = _mapper.Map<ReadDestinationDto>(dest);
            destDto.Photos = new List<string>();
            foreach (var photo in dest.Photos)
            {
                destDto.Photos.Add(this.GetDestinationPhotoEndpointUrl(dest.Id, photo.Id));
            }

            destinationDtos.Add(destDto);
        }

        return destinationDtos;
    }

    public IEnumerable<ReadDestinationDto> GetAll()
    {
        var destinations = _appDbContext.Destinations.ToList();

        return this.DestinationsToDto(destinations);
    }

    public IEnumerable<ReadDestinationDto> GetAll(string name)
    {
        var destinations = _appDbContext.Destinations
        .Where(dest => EF.Functions.Like(dest.Name, $"%{name}%"))
        .ToList();

        return this.DestinationsToDto(destinations);
    }

    public ReadDestinationDto GetById(int id)
    {
        var destination = _appDbContext.Destinations.FirstOrDefault(destination => destination.Id == id)
        ?? throw new Destination.DoesNotExists($"Destino {id} não foi localizado");

        var destinationDto = _mapper.Map<ReadDestinationDto>(destination);

        foreach (var photo in destination.Photos)
        {

            destinationDto.Photos.Add(this.GetDestinationPhotoEndpointUrl(destination.Id, photo.Id));
        }

        return destinationDto;
    }

    /// <summary>
    /// Obtém o caminho da foto no diretório
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="Destination.DoesNotExists"></exception>
    public List<string> GetPhotosDirectory(int id)
    {
        var destination = _appDbContext.Destinations.FirstOrDefault(destination => destination.Id == id)
        ?? throw new Destination.DoesNotExists($"Destino {id} não foi localizado");

        List<string> photosDirectory = new();

        foreach (var photoFile in destination.Photos)
        {
            photosDirectory.Add(photoFile.Photo);
        }

        return photosDirectory;
    }

    public async Task<ReadDestinationDto> Register(CreateDestinationDto destinationDto, List<IFormFile> photos)
    {
        var destination = _mapper.Map<Destination>(destinationDto);

        if (string.IsNullOrEmpty(destination.DescritiveText))
        {
            //TODO: Save GPT answer in destination.DescritiveText
            var result = await _openAIService.SendMessage($"Faça um resumo sobre {destination.Name} enfatizando o porque este lugar é incrível. Utilize uma linguagem informal e até 100 caracteres no máximo em cada parágrafo. Crie 2 parágrafos neste resumo.");
        }

        foreach (var photoFile in photos)
        {
            var photo = new Photos()
            {
                DestinationId = destination.Id,
                Destination = destination,
                Photo = _fileManager.SaveFile(BASE_PHOTO_PATH, photoFile)
            };

            destination.Photos.Add(photo);
        }

        _appDbContext.Destinations.Add(destination);

        _appDbContext.SaveChanges();

        var readDestDto = _mapper.Map<ReadDestinationDto>(destination);

        foreach (var photo in destination.Photos)
        {

            readDestDto.Photos.Add(this.GetDestinationPhotoEndpointUrl(destination.Id, photo.Id));
        }

        return readDestDto;
    }

    public ReadDestinationDto Update(int id, UpdateDestinationDto destinationDto, List<IFormFile>? photos)
    {
        var destination = _appDbContext.Destinations.FirstOrDefault(destination => destination.Id == id)
       ?? throw new Destination.DoesNotExists($"Destino {id} não foi localizado");

        _mapper.Map(destinationDto, destination);

        if (photos != null)
        {
            //Remove the old photos
            foreach (var destPhoto in destination.Photos)
            {
                _appDbContext.Photos.Remove(destPhoto);

                _fileManager.Remove(destPhoto.Photo);
            }

            // Save the new files
            foreach (var photoFile in photos)
            {
                var photo = new Photos()
                {
                    Destination = destination,
                    Photo = _fileManager.SaveFile(BASE_PHOTO_PATH, photoFile)
                };

                _appDbContext.Photos.Add(photo);
            }
        }

        _appDbContext.SaveChanges();

        var readDestDto = _mapper.Map<ReadDestinationDto>(destination);

        foreach (var photo in destination.Photos)
        {

            readDestDto.Photos.Add(this.GetDestinationPhotoEndpointUrl(destination.Id, photo.Id));
        }

        return readDestDto;
    }

    public void Delete(int id)
    {
        var destination = _appDbContext.Destinations.FirstOrDefault(dest => dest.Id == id) ??
        throw new Destination.DoesNotExists($"Destino {id} não localizado");

        _appDbContext.Destinations.Remove(destination);

        foreach (var photoFile in destination.Photos)
        {
            //Remove the destination photo
            _appDbContext.Photos.Remove(photoFile);

            _fileManager.Remove(photoFile.Photo);
        }

        _appDbContext.SaveChanges();
    }

    public FileStream GetPhoto(int destinatioId, int photoId)
    {
        var photo = _appDbContext.Photos.FirstOrDefault(photo => photo.Id == photoId && photo.DestinationId == destinatioId) ??
        throw new Photos.DoesNotExists($"Foto {photoId} não localizada");

        return _fileManager.GetPhoto(photo.Photo);
    }
}
