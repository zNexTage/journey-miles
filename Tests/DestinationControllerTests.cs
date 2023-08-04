using API.Controllers;
using API.DTO.Destination;
using API.Models;
using API.Service.Providers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests;

public class DestinationControllerTests
{

    private List<ReadDestinationDto> GetTestDestinations()
    {
        var destinations = new List<ReadDestinationDto>();

        destinations.Add(new ReadDestinationDto()
        {
            Id = 1,
            Name = "Rio de Janeiro",
            Photo = "rio.png",
            Price = 150
        });

        destinations.Add(new ReadDestinationDto()
        {
            Id = 2,
            Name = "São Paulo",
            Photo = "sp.png",
            Price = 200
        });

        destinations.Add(new ReadDestinationDto()
        {
            Id = 3,
            Name = "Ceará",
            Photo = "ceara.png",
            Price = 444
        });

        return destinations;
    }

    [Fact]
    public void Get_ReturnAllDestinations(){
        //Arrange
        Mock<IDestinationService> mockService = new();
        mockService.Setup(service => service.GetAll()).Returns(this.GetTestDestinations());

        var controller = new DestinationController(mockService.Object);

        //Act
        var result = (OkObjectResult)controller.GetAll(null);
        
        //Assert
        Assert.True(result.StatusCode == 200);
        Assert.IsType<List<ReadDestinationDto>>(result.Value);
    }

    [Fact]
    public void Get_DestinationById(){
        //Arrange
        Mock<IDestinationService> mockService = new();
        mockService.Setup(service => service.GetById(1)).Returns(new ReadDestinationDto() {
            Id = 1,
            Name = "São Paulo",
            Photo = "https://localhost:4717/sp.png",
            Price = 150
        });

        var controller = new DestinationController(mockService.Object);

        //Act
        var result = (OkObjectResult)controller.GetById(1);

        //Assert
        Assert.True(result.StatusCode == 200);
        Assert.IsType<ReadDestinationDto>(result.Value);        
    }

    [Fact]
    public void  Get_DestinationWithInvalidId(){
        //Arrange
        Mock<IDestinationService> mockService = new();
        mockService.Setup(service => service.GetById(1)).Throws(new Destination.DoesNotExists("Destino 1 não foi localizado"));

        var controller = new DestinationController(mockService.Object);

        //Act
        var result = (NotFoundObjectResult)controller.GetById(1);

        //Assert
        Assert.True(result.StatusCode == 404);
    }

    [Fact]
    public void Post_CreateDestination(){
        //Arrange
        CreateDestinationDto depositionDto = new(){
            Name = "São Paulo",
            Price = 150
        };

        var fileMock = new Mock<IFormFile>();

        fileMock.Setup(file=> file.FileName).Returns("sp.png");
        fileMock.Setup(file=> file.Length).Returns(100);

        var expectedReturn = new ReadDestinationDto(){
            Name = "Descrição",
            Price = 150,
            Photo = "sp.png"
        };

        Mock<IDestinationService> mockService = new();
        mockService.Setup(service => service.Register(depositionDto, fileMock.Object))
        .Returns(expectedReturn);

        //Act
        var controller = new DestinationController(mockService.Object);

        var result = (CreatedAtActionResult)controller.Register(depositionDto, fileMock.Object);

        //Arrange
        Assert.True(result.StatusCode == 201);
    }

    [Fact]
    public void Delete_RemoveDestination(){
        //Arrange
        Mock<IDestinationService> mockService = new();
        mockService.Setup(service => service.Delete(1));

        var controller = new DestinationController(mockService.Object);

        //Act
        var result = (NoContentResult)controller.Delete(1);

        //Assert
        Assert.True(result.StatusCode == 204);
    }

    [Fact]
    public void Delete_RemoveDestination_WithInvalidId(){
        //Arrange
        Mock<IDestinationService> mockService = new();
        mockService.Setup(service => service.Delete(1)).Throws(new Destination.DoesNotExists("Destino 1 não foi localizado"));

        var controller = new DestinationController(mockService.Object);

        //Act
        var result = (NotFoundObjectResult)controller.Delete(1);

        //Assert
        Assert.True(result.StatusCode == 404);
    }

    [Fact]
    public void Put_UpdateDeposition(){

        //Arrange
        var updateDestinationDto = new UpdateDestinationDto() {
            Name = "São Paulo",
            Price = 120
        };


        var fileMock = new Mock<IFormFile>();

        fileMock.Setup(file=> file.FileName).Returns("sp.png");
        fileMock.Setup(file=> file.Length).Returns(100);

        Mock<IDestinationService> mockService = new();
        mockService.Setup(service => service.Update(1, updateDestinationDto, fileMock.Object))
        .Returns(new ReadDestinationDto() {
            Name = "São Paulo - Itu",
            Price = 200
        });

        var controller = new DestinationController(mockService.Object);

        // Act
        var result = (OkObjectResult)controller.Update(1, updateDestinationDto, fileMock.Object);

        // Assert
        Assert.True(result.StatusCode == 200);
        Assert.IsType<ReadDestinationDto>(result.Value);
    }

    [Fact]
    public void Put_UpdateDeposition_WithInvalidId(){

        //Arrange
        var updateDestinationDto = new UpdateDestinationDto() {
            Name = "São Paulo",
            Price = 120
        };


        var fileMock = new Mock<IFormFile>();

        fileMock.Setup(file=> file.FileName).Returns("sp.png");
        fileMock.Setup(file=> file.Length).Returns(100);

        Mock<IDestinationService> mockService = new();
        mockService.Setup(service => service.Update(1, updateDestinationDto, fileMock.Object))
        .Throws(new Destination.DoesNotExists("Destino 1 não localizado"));

        var controller = new DestinationController(mockService.Object);

        // Act
        var result = (NotFoundObjectResult)controller.Update(1, updateDestinationDto, fileMock.Object);

        // Assert
        Assert.True(result.StatusCode == 404);
    }
}
