using System;
using API.Controllers;
using API.DTO.Deposition;
using API.Models;
using API.Utils;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace API.Service;

public class DepositionService
{
    private AppDbContext _appDbContext;
    private IMapper _mapper;
    private FileManager _fileManager;
    private IUrlHelper _urlHelper;
    private IHttpContextAccessor _httpRequest;

    public DepositionService(AppDbContext appDbContext,
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
    /// Retorna a URL da foto de um depoimento.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private string GetDepositionPhotoEndpointUrl(int id)
    {
        var controllerName = nameof(DepositionController).Replace("Controller", string.Empty);
        var methodName = "GetPhoto";        

        var url = _urlHelper.Action(
                methodName,
                controllerName,
                new { id },
                _httpRequest.HttpContext.Request.Scheme
        );

        return url;
    }

    public IEnumerable<ReadDepositionDto> GetAll()
    {
        var depositions = _appDbContext.Depositions.ToList();

        var depositionsDto = _mapper.Map<List<ReadDepositionDto>>(depositions);

        foreach(var deposition in depositionsDto){
            deposition.Photo = GetDepositionPhotoEndpointUrl(deposition.Id);
        }

        return depositionsDto;
    }

    public ReadDepositionDto Get(int id)
    {
        var deposition = _appDbContext.Depositions.FirstOrDefault(depo => depo.Id == id) ?? throw new Deposition.DoesNotExists($"Depoimento {id} não foi localizado");

        var dto = _mapper.Map<ReadDepositionDto>(deposition);

        dto.Photo = GetDepositionPhotoEndpointUrl(dto.Id);

        return dto;
    }

    public ReadDepositionDto Register(CreateDepositionDto depositionDto, IFormFile photo)
    {
        var fileExtesion = Path.GetExtension(photo.FileName);
        //Salva a foto no diretório e obtém o caminho.
        var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + fileExtesion;

        var fullPath = _fileManager.SaveFile(fileName, photo);

        var deposition = _mapper.Map<Deposition>(depositionDto);
        deposition.Photo = fullPath;

        _appDbContext.Depositions.Add(deposition);
        _appDbContext.SaveChanges();

        return _mapper.Map<ReadDepositionDto>(deposition);
    }

    /// <summary>
    /// Obtem o caminho da foto do depoimento salvo no diretório.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="Deposition.DoesNotExists"></exception>
    public string GetPhotoDirectory(int id){
        var deposition = _appDbContext.Depositions
        .FirstOrDefault(depo => depo.Id == id)
        ?? throw new Deposition.DoesNotExists($"Depoimento {id} não foi localizado");

        return deposition.Photo;
    }
}
