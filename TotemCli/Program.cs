using System.CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using TotemCli.Services;
using System.Security.Cryptography.X509Certificates;
using TotemCli.CommandHandlers;
using TotemCli.Src.Services;

public partial class Program
{
    private readonly IConfiguration _configuration;
    private readonly IServiceProvider _serviceProvider;
    public Program()
    {
        _configuration = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();
        
        _serviceProvider = new ServiceCollection()
            .AddSingleton(_configuration)
            .AddSingleton<AssetService>()
            .AddSingleton<DestinationService>()
            .AddSingleton<LoadAssetHandler>()
            .AddSingleton<CreateDestinationHandler>()
            .BuildServiceProvider();
    }


    static async Task<int> Main(string[] args)
    {
        var program = new Program();

        var rootCommand = new RootCommand();

        program._serviceProvider.GetRequiredService<LoadAssetHandler>().AddCommand(rootCommand);
        program._serviceProvider.GetRequiredService<CreateDestinationHandler>().AddCommand(rootCommand);

        return await rootCommand.InvokeAsync(args);
    }


}