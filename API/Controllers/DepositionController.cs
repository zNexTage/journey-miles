using System;
using API.DTO.Deposition;
using API.Service;
using Microsoft.AspNetCore.Http;
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
        nameof(DepositionController),
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
        return Ok();
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
}
