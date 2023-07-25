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

    public DestinationController(IDestinationService destinationService){
        _destinationService = destinationService;
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
    public IActionResult Register([FromForm]CreateDestinationDto depositionDto, IFormFile photo){
        var destination = _destinationService.Register(depositionDto, photo);

         return CreatedAtAction(nameof(GetById),
            new { id = destination.Id },
            destination
        );
    }

    
}