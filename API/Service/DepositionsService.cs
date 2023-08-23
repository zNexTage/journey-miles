using System;
using API.Controllers;
using API.DTO.Deposition;
using API.Models;
using API.Service.Providers;
using API.Utils;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;

namespace API.Service;

public class DepositionService : IDepositionService
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

        foreach (var deposition in depositionsDto)
        {
            deposition.Photo = GetDepositionPhotoEndpointUrl(deposition.Id);
        }

        return depositionsDto;
    }

    /// <summary>
    /// Obtém três depoimentos aleatórios
    /// TODO: A quantidade de itens deveria ser parametrizado?
    /// </summary>
    /// <returns></returns>
    public IEnumerable<ReadDepositionDto> GetRandom(){
        var depositions = _appDbContext.Depositions.OrderBy(r => EF.Functions.Random()).Take(3);

        return _mapper.Map<List<ReadDepositionDto>>(depositions);
    }

    public string SavePhoto(IFormFile photo){
        //Salva a foto no diretório e obtém o caminho.
        var fullPath = _fileManager.SaveFile("destinations", photo);

        return fullPath;
    }

    public ReadDepositionDto Get(int id)
    {
        var deposition = _appDbContext.Depositions.FirstOrDefault(depo => depo.Id == id) 
        ?? throw new Deposition.DoesNotExists($"Depoimento {id} não foi localizado");

        var dto = _mapper.Map<ReadDepositionDto>(deposition);

        dto.Photo = GetDepositionPhotoEndpointUrl(dto.Id);

        return dto;
    }
    

    public ReadDepositionDto Register(CreateDepositionDto depositionDto, IFormFile photo)
    {
        var fullPath = SavePhoto(photo);

        var deposition = _mapper.Map<Deposition>(depositionDto);
        deposition.Photo = fullPath;

        _appDbContext.Depositions.Add(deposition);
        _appDbContext.SaveChanges();

        var readDepoDto = _mapper.Map<ReadDepositionDto>(deposition);
        readDepoDto.Photo = this.GetDepositionPhotoEndpointUrl(deposition.Id);

        return readDepoDto;
    }

    public ReadDepositionDto Update(int id, UpdateDepositionDto depositionDto, IFormFile? photo)
    {
        var deposition = _appDbContext.Depositions.FirstOrDefault(depo => depo.Id == id);

        if(deposition == null){
            throw new Deposition.DoesNotExists($"Depoimento de id {id} não existe");
        }

        _mapper.Map(depositionDto, deposition);

        if (photo != null)
        {
            //Remove a imagem antiga
            _fileManager.Remove(deposition.Photo);

            // Salva a imagem nova
            var fullPath = SavePhoto(photo);
            deposition.Photo = fullPath;    
        }

        _appDbContext.SaveChanges();

        deposition.Photo = GetDepositionPhotoEndpointUrl(deposition.Id);

        return _mapper.Map<ReadDepositionDto>(deposition);
    }

    public void Delete(int id)
    {
        var deposition = _appDbContext.Depositions.FirstOrDefault(depo => depo.Id == id);

        if(deposition == null){
            throw new Deposition.DoesNotExists($"Depoimento de id {id} não existe");
        }

        _appDbContext.Depositions.Remove(deposition);

        _appDbContext.SaveChanges();
    }

    public FileStream GetPhoto(int id){
        var depositions = _appDbContext.Depositions.FirstOrDefault(depo => depo.Id == id)
        ?? throw new Deposition.DoesNotExists($"Destino {id} não foi localizado");

        return _fileManager.GetPhoto(depositions.Photo);
    }

}
