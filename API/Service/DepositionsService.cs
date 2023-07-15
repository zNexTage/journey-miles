using System;
using API.DTO.Deposition;
using API.Models;
using API.Utils;
using AutoMapper;

namespace API.Service;

public class DepositionService
{
    private AppDbContext _appDbContext;
    private IMapper _mapper;
    private FileManager _fileManager;

    public DepositionService(AppDbContext appDbContext, IMapper mapper, IWebHostEnvironment environment)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
        _fileManager = new FileManager(environment);
    }

    public IEnumerable<Deposition> GetAll()
    {
        var depositions = _appDbContext.Depositions.ToList();

        return _mapper.Map<List<Deposition>>(depositions);
    }

    public Deposition Get(int id)
    {
        var deposition = _appDbContext.Depositions.Where(depo => depo.Id == id);

        return _mapper.Map<Deposition>(deposition);
    }

    public Deposition Register(CreateDepositionDto depositionDto, IFormFile photo)
    {
        var fileExtesion = Path.GetExtension(photo.FileName);
        //Salva a foto no diretório e obtém o caminho.
        var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + fileExtesion;
        
        var fullPath = _fileManager.SaveFile(fileName, photo);

        var deposition = _mapper.Map<Deposition>(depositionDto);
        deposition.Photo = fullPath;

        _appDbContext.Depositions.Add(deposition);
        _appDbContext.SaveChanges();

        return deposition;
    }
}
