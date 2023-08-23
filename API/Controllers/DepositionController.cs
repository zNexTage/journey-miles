using System;
using API.DTO.Deposition;
using API.Models;
using API.Service;
using API.Service.Providers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("/api/depoimentos")]
[ApiController]
public class DepositionController : ControllerBase
{
    private IDepositionService _depositionService;

    public DepositionController(IDepositionService depositionService)
    {
        _depositionService = depositionService;
    }

    /// <summary>
    /// Validate and register a Deposition 
    /// </summary>
    /// <param name="depositionDto"></param>
    /// <param name="photo"></param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult Create([FromForm] CreateDepositionDto depositionDto, IFormFile photo)
    {
        var deposition = _depositionService.Register(depositionDto, photo);

        return CreatedAtAction(nameof(GetById),
        new { id = deposition.Id },
        deposition
        );
    }

    /// <summary>
    /// Get all deposition saved in database
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(
            _depositionService.GetAll()
        );
    }

    /// <summary>
    /// Get three random deposition
    /// </summary>
    /// <returns></returns>
    [HttpGet("home/")]
    public IActionResult GetRandom(){
        return Ok(_depositionService.GetRandom());
    }

    /// <summary>
    /// Get a deposition by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Update a specific deposition. When a picture is send in request, the photo saved will replace the older.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="depositionDto"></param>
    /// <param name="photo"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Remove a specific deposition using the id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
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
    /// Serve a deposition image
    /// Ref: https://stackoverflow.com/questions/40794275/return-jpeg-image-from-asp-net-core-webapi
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("foto/{id}")]
    public IActionResult GetPhoto(int id)
    {
        try
            {
                var photo = _depositionService.GetPhoto(id);

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
