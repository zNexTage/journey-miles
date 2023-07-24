using API.Controllers;
using API.DTO.Deposition;
using API.Models;
using API.Service;
using API.Service.Providers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests;


public class DepositionControllerTests
{


    private List<ReadDepositionDto> GetTestDepositions()
    {
        var depositions = new List<ReadDepositionDto>();

        depositions.Add(new ReadDepositionDto()
        {
            Id = 1,
            Description = "Bem bacana",
            PersonName = "Christian",
            Photo = "foto.png"
        });

        depositions.Add(new ReadDepositionDto()
        {
            Id = 2,
            Description = "Muito legal",
            PersonName = "Alfredo",
            Photo = "foto_2.png"
        });

        depositions.Add(new ReadDepositionDto()
        {
            Id = 3,
            Description = "Bom demais",
            PersonName = "Bender",
            Photo = "foto_3.png"
        });

        return depositions;
    }

    [Fact]
    public void Get_ReturnAllDepositions()
    {
        //Arrange
        Mock<IDepositionService> mockService = new();
        mockService.Setup(service => service.GetAll()).Returns(this.GetTestDepositions());

        var controller = new DepositionController(mockService.Object);

        // Act
        var result = (OkObjectResult)controller.GetAll();

        //Assert
        Assert.True(result.StatusCode == 200);
        Assert.IsType<List<ReadDepositionDto>>(result.Value);
    }

    [Fact]
    public void Get_ReturnADeposition()
    {
        //Arrange
        var deposition = new ReadDepositionDto()
        {
            Id = 3,
            Description = "Bom demais",
            PersonName = "Bender",
            Photo = "foto_3.png"
        };

        Mock<IDepositionService> mockService = new();
        mockService.Setup(service => service.Get(1))
        .Returns(deposition);

        var controller = new DepositionController(mockService.Object);

        //Act
        var result = (OkObjectResult)controller.GetById(1);

        //Assert
        Assert.True(result.StatusCode == 200);
        Assert.IsType<ReadDepositionDto>(result.Value);
    }

    [Fact]
    public void Get_With_InvalidId(){
        //Arrange
        var deposition = new ReadDepositionDto()
        {
            Id = 3,
            Description = "Bom demais",
            PersonName = "Bender",
            Photo = "foto_3.png"
        };

        Mock<IDepositionService> mockService = new();
        mockService.Setup(service => service.Get(555))
        .Throws(new Deposition.DoesNotExists("Deposition 555 not exists"));
        var controller = new DepositionController(mockService.Object);
        
        //Act
        var result = (NotFoundObjectResult)controller.GetById(555);        

        //Assert
        Assert.True(result.StatusCode == 404);
    }

    [Fact]
    public void Get_RandomDeposition(){
        //Arrage
        Mock<IDepositionService> mockService = new();
        mockService.Setup(service => service.GetRandom())
        .Returns(this.GetTestDepositions());

        var controller = new DepositionController(mockService.Object);

        //Act
        var result = (OkObjectResult)controller.GetRandom();

        //Assert
        Assert.True(result.StatusCode == 200);
        Assert.IsType<List<ReadDepositionDto>>(result.Value);
    }    

    [Fact]
    public void Post_CreateDeposition(){
        //Assert
        CreateDepositionDto depositionDto = new CreateDepositionDto(){
            Description = "Descrição",
            PersonName = "Christian"
        };

        var fileMock = new Mock<IFormFile>();

        fileMock.Setup(file=> file.FileName).Returns("arquivo.png");
        fileMock.Setup(file=> file.Length).Returns(100);

        var expectedReturn = new ReadDepositionDto(){
            Description = "Descrição",
            PersonName = "Christian",
            Photo = "arquivo.png"
        };

        Mock<IDepositionService> mockService = new();
        mockService.Setup(service => service.Register(depositionDto, fileMock.Object))
        .Returns(expectedReturn);

        var controller = new DepositionController(mockService.Object);

        //Act
        var result = (CreatedAtActionResult)controller.Create(depositionDto, fileMock.Object);

        //Assert
        Assert.True(result.StatusCode == 201);
        Assert.IsType<ReadDepositionDto>(result.Value);
    }

    [Fact]
    public void Delete_RemoveDeposition(){
        //Arrange
        Mock<IDepositionService> mockService = new();
        mockService.Setup(service => service.Delete(1));

        var controller = new DepositionController(mockService.Object);

        //Act
        var result = (NoContentResult)controller.Delete(1);

        //Assert
        Assert.True(result.StatusCode == 204);
    }

    [Fact]
    public void Delete_RemoveDeposition_With_InvalidId(){
        //Arrange
        Mock<IDepositionService> mockService = new();
        mockService.Setup(service => service.Delete(1))
        .Throws(new Deposition.DoesNotExists("Deposition 1 does not exists"));

        var controller = new DepositionController(mockService.Object);

        //Act
        var result = (NotFoundObjectResult)controller.Delete(1);

        //Assert
        Assert.True(result.StatusCode == 404);
    }

    [Fact]
    public void Put_UpdateDeposition(){

        //Arrange
        var updateDepositionDto = new UpdateDepositionDto() {
            Description = "Teste",
            PersonName = "Christian"
        };


        var fileMock = new Mock<IFormFile>();

        fileMock.Setup(file=> file.FileName).Returns("arquivo.png");
        fileMock.Setup(file=> file.Length).Returns(100);

        Mock<IDepositionService> mockService = new();
        mockService.Setup(service => service.Update(1, updateDepositionDto, fileMock.Object))
        .Returns(new ReadDepositionDto() {
            Description = "Teste",
            PersonName = "Christian"
        });

        var controller = new DepositionController(mockService.Object);

        // Act
        var result = (OkObjectResult)controller.Update(1, updateDepositionDto, fileMock.Object);

        // Assert
        Assert.True(result.StatusCode == 200);
        Assert.IsType<ReadDepositionDto>(result.Value);
    }

    [Fact]
    public void Put_UpdateDeposition_With_InvalidId(){
        //Arrange
        var updateDepositionDto = new UpdateDepositionDto() {
            Description = "Teste",
            PersonName = "Christian"
        };

        var fileMock = new Mock<IFormFile>();

        fileMock.Setup(file=> file.FileName).Returns("arquivo.png");
        fileMock.Setup(file=> file.Length).Returns(100);

        Mock<IDepositionService> mockService = new();
        mockService.Setup(service => service.Update(1, updateDepositionDto, fileMock.Object))
        .Throws(new Deposition.DoesNotExists("Deposition 2 does not exists"));
        
        var controller = new DepositionController(mockService.Object);

        //Act
        var result = (NotFoundObjectResult)controller.Update(1, updateDepositionDto, fileMock.Object);

        //Assert
        Assert.True(result.StatusCode == 404);
    }
}