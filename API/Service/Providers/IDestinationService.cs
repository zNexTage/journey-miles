using System;
using API.DTO.Destination;

namespace API.Service.Providers;

public interface IDestinationService
{
    IEnumerable<ReadDestinationDto> GetAll();
    IEnumerable<ReadDestinationDto> GetAll(string name);
    ReadDestinationDto GetById(int id);

    ReadDestinationDto Register(CreateDestinationDto destinationDto, List<IFormFile> photos);

    ReadDestinationDto Update(int id, UpdateDestinationDto destinationDto, List<IFormFile>? photos);

    void Delete(int id);

    List<string> GetPhotosDirectory(int id);

    FileStream GetPhoto(int destinatioId, int photoId);
}