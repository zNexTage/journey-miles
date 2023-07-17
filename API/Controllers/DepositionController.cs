using System;
using API.DTO.Deposition;
using API.Models;
using API.Service;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("/api/depoimentos")]
[ApiController]
public class DepositionController : ControllerBase
{
    private DepositionService _depositionService;

    public DepositionController(DepositionService depositionService)
    {
        _depositionService = depositionService;
    }

    [HttpPost]
    public IActionResult Create([FromForm] CreateDepositionDto depositionDto, IFormFile photo)
    {
        var deposition = _depositionService.Register(depositionDto, photo);

        return CreatedAtAction(nameof(GetById),
        new { id = deposition.Id },
        deposition
        );
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(
            _depositionService.GetAll()
        );
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        try
        {
            var deposition = _depositionService.Get(id);

            return Ok(deposition);
        }
        catch (Deposition.DoesNotExists)
        {
            return NotFound($"Depoimento {id} não localizado");
        }
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromForm] UpdateDepositionDto depositionDto, IFormFile? photo)
    {
        try{
            var deposition = _depositionService.Update(id, depositionDto, photo);

            return Ok(deposition);
        }
        catch(Deposition.DoesNotExists err){
            return NotFound($"Depoimento {id} não localizado");
        }

    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        try{
            _depositionService.Delete(id);

            return NoContent();
        }
        catch(Deposition.DoesNotExists err){
            return NotFound($"Depoimento {id} não localizado");
        }
    }

    /// <summary>
    /// Retorna a imagem de um depoimento;
    /// Ref: https://stackoverflow.com/questions/40794275/return-jpeg-image-from-asp-net-core-webapi
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("foto/{id}")]
    public IActionResult GetPhoto(int id)
    {
        try
            {
                var photoDirectory = _depositionService.GetPhotoDirectory(id);
                
                var photo = System.IO.File.OpenRead(photoDirectory);

                return File(photo, "image/jpg");
            }
            catch (Exception err)
            {
                if (err is FileNotFoundException || 
                err is DirectoryNotFoundException || 
                err is Deposition.DoesNotExists)
                {
                    return NotFound("Imagem não localizada");
                }

                return StatusCode(500,
                new { error = "Ocorreu um erro ao obter a imagem do depoimento" });
            }
    }
}
