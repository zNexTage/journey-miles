using System;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API;

public class AppDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

    public AppDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public DbSet<Deposition> Depositions { get; set; }
    public DbSet<Destination> Destinations { get; set; }

    public DbSet<Photos> Photos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = _configuration.GetConnectionString("Mysql");

        optionsBuilder
        .UseLazyLoadingProxies()
        .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder){
        
        // Relatinship between Destination and Photo
        modelBuilder.Entity<Destination>()
        .HasMany(dest => dest.Photos)
        .WithOne(photo => photo.Destination)
        .HasForeignKey(photo => photo.DestinationId)
        .HasPrincipalKey(photo => photo.Id);
    }
}
