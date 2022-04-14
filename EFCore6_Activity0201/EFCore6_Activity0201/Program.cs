// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;

IConfigurationRoot _configuration = BuildConfiguration();
Console.WriteLine($"CNSTR: {_configuration.GetConnectionString("AdventureWorks")}");

IConfigurationRoot BuildConfiguration()
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

    return builder.Build();
}