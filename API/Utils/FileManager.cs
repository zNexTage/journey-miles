using System;

namespace API.Utils;
public class FileManager
{
    private string BASE_PATH = "";

    public FileManager(IWebHostEnvironment environment)
    {
        //Monta o camainho base onde será armazenados os arquivos
        BASE_PATH = Path.Combine(environment.ContentRootPath, "media");

        if(!Directory.Exists(BASE_PATH)){
            Directory.CreateDirectory(BASE_PATH);
        }
    }

    /// <summary>
    /// Salva um arquivo enviado em uma requisição nos diretórios da aplicação
    /// </summary>
    /// <param name="filename"></param>
    /// <param name="file"></param>
    /// <returns></returns>
    public string SaveFile(string basePath, IFormFile file)
    {
        var fileExtesion = Path.GetExtension(file.FileName);
        var filename = GetPhotoFilename(fileExtesion);

        var path = Path.Combine(BASE_PATH, basePath);

        if(!Directory.Exists(path)){
            Directory.CreateDirectory(path);
        }

        var fullpath = Path.Combine(path, filename);

        using (Stream fileStream = new FileStream(fullpath, FileMode.Create))
        {
            file.CopyTo(fileStream);
        }

        return fullpath;
    }

    public void Remove(string path){
        if(File.Exists(path)){
            File.Delete(path);
        }
    }

    private string GetPhotoFilename(string fileExtesion)
    {
        return Guid.NewGuid().ToString() + fileExtesion;
    }

    public FileStream GetPhoto(string path){
        return System.IO.File.OpenRead(path);
    }
}
