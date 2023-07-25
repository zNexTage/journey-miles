using System;
using API.DTO.Deposition;

namespace API.Service.Providers;

public interface IDepositionService
{   

    IEnumerable<ReadDepositionDto> GetAll();

    IEnumerable<ReadDepositionDto> GetRandom();
    
    string GetPhotoDirectory(int id);

    string SavePhoto(IFormFile photo);

    ReadDepositionDto Get(int id);

    ReadDepositionDto Register(CreateDepositionDto depositionDto, IFormFile photo);

    ReadDepositionDto Update(int id, UpdateDepositionDto depositionDto, IFormFile? photo);

    void Delete(int id);
}