using System;
using API.DTO.Destination;
using API.Models;
using API.Service.Providers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("/api/destinos")]
[ApiController]
public class DestinationController : ControllerBase
{
    private readonly IDestinationService _destinationService;  
    private readonly IWebHostEnvironment _env;

    public DestinationController(IDestinationService destinationService){
        _destinationService = destinationService;
    }

    [HttpGet]
    public IActionResult GetAll([FromQuery] string? name){
        if(string.IsNullOrEmpty(name)){
            return Ok(_destinationService.GetAll());
        } 

        return Ok(_destinationService.GetAll(name));
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
    public IActionResult Register([FromForm]CreateDestinationDto destinationDto, List<IFormFile> photos){
        var destination = _destinationService.Register(destinationDto, photos);

         return CreatedAtAction(nameof(GetById),
            new { id = destination.Id },
            destination
        );
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromForm]UpdateDestinationDto destinationDto, List<IFormFile>? photos){
        try{
            var destination = _destinationService.Update(id, destinationDto, photos);

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
    [HttpGet("{destinationId}/foto/{photoId}")]
    public IActionResult GetPhoto(int destinationId, int photoId)
    {
        try
            {
                var photo = _destinationService.GetPhoto(destinationId, photoId);   

                return File(photo, "image/jpg");
            }
            catch (Exception err)
            {
                if (err is FileNotFoundException || 
                err is DirectoryNotFoundException || 
                err is Photos.DoesNotExists)
                {
                    return NotFound("Imagem não localizada");
                }

                return StatusCode(500,
                new { error = "Ocorreu um erro ao obter a imagem do destino" });
            }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id){
        try{
            _destinationService.Delete(id);
            
            return NoContent();
        }
        catch (Deposition.DoesNotExists err){
            return NotFound(err.Message);
        }
    }
}