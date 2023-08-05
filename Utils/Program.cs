// See https://aka.ms/new-console-template for more information
using API;
using API.Models;
using Microsoft.Extensions.Configuration;
using Utils;

IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Development.json")
            .Build();

MigratePhotoToPhotos.Execute(configuration);

