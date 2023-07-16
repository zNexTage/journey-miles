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
        return Ok();
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
            return NotFound("Imagem não localizada");
        }
    }

    [HttpPut]
    public IActionResult Update(int id)
    {
        return Ok();
    }

    [HttpDelete]
    public IActionResult Delete(int id)
    {
        return NoContent();
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
            var deposition = _depositionService.Get(id);

            var photo = System.IO.File.OpenRead(deposition.Photo);

            return File(photo, "image/jpg");
        }
        catch (Deposition.DoesNotExists)
        {
            return NotFound("Imagem não localizada");
        }
    }
}
