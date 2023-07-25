using System;
using API.DTO.Destination;
using API.Models;
using API.Service.Providers;
using API.Utils;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("/api/destinos")]
[ApiController]
public class DestinationController : ControllerBase
{
    private readonly IDestinationService _destinationService;  
    private readonly FileManager _fileManager; 
    private readonly IWebHostEnvironment _env;

    public DestinationController(IDestinationService destinationService, IWebHostEnvironment webHostEnvironment){
        _destinationService = destinationService;
        _fileManager = new FileManager(webHostEnvironment);
    }

    [HttpGet]
    public IActionResult GetAll(){
        return Ok(_destinationService.GetAll());
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id){
        try{
            var destination = _destinationService.GetById(id);

            return Ok(destination);
        }
        catch(Destination.DoesNotExists ex){
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    public IActionResult Register([FromForm]CreateDestinationDto destinationDto, IFormFile photo){
        var destination = _destinationService.Register(destinationDto, photo);

         return CreatedAtAction(nameof(GetById),
            new { id = destination.Id },
            destination
        );
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromForm]UpdateDestinationDto destinationDto, IFormFile photo){
        try{
            var destination = _destinationService.Update(id, destinationDto, photo);

            return Ok(destination);
        }
        catch(Destination.DoesNotExists err){
            return NotFound(err.Message);   
        }

    }

    /// <summary>
    /// Retorna a imagem de um destino;
    /// Ref: https://stackoverflow.com/questions/40794275/return-jpeg-image-from-asp-net-core-webapi
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("foto/{id}")]
    public IActionResult GetPhoto(int id)
    {
        try
            {
                var photoPath = _destinationService.GetPhotoDirectory(id);                
                
                var photo = _fileManager.GetPhoto(photoPath);

                return File(photo, "image/jpg");
            }
            catch (Exception err)
            {
                if (err is FileNotFoundException || 
                err is DirectoryNotFoundException || 
                err is Destination.DoesNotExists)
                {
                    return NotFound("Imagem não localizada");
                }

                return StatusCode(500,
                new { error = "Ocorreu um erro ao obter a imagem do destino" });
            }
    }

    
}