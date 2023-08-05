using API;
using Microsoft.Extensions.Configuration;

namespace Utils;

public class MigratePhotoToPhotos
{
    public static void Execute(IConfiguration configuration)
    {
        AppDbContext appDbContext = new(configuration);

        var destinations = appDbContext.Destinations.ToList();

        foreach (var dest in destinations)
        {
            // Photos photos = new () {
            //     Destination = dest,
            //     DestinationId = dest.Id,
            //     Photo = dest.Photo
            // };

            // appDbContext.Photos.Add(photos);
            // dest.Photo = "";
            // appDbContext.SaveChanges();
            // Console.WriteLine(dest.Photo);
            foreach(var photo in dest.Photos){
                Console.WriteLine(photo.Id);
                Console.WriteLine(photo.Photo);
            }
        }
    }
}
