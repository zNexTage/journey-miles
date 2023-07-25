using System;
using API.DTO.Destination;

namespace API.Service.Providers;

public interface IDestinationService
{
    IEnumerable<ReadDestinationDto> GetAll();
    ReadDestinationDto GetById(int id);

    ReadDestinationDto Register(CreateDestinationDto destinationDto, IFormFile photo);

    ReadDestinationDto Update(int id, UpdateDestinationDto destinationDto, IFormFile? photo);

    void Delete(int id);

    string GetPhotoDirectory(int id);
}