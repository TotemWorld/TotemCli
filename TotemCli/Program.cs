using System.CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using TotemCli.Services;
using System.Security.Cryptography.X509Certificates;
using TotemCli.CommandHandlers;
using TotemCli.Src.Services;
using Spectre.Console;
using Spectre.Console.Cli;
using TotemCli.CommandHandler;
using TotemCli.PromptHandler;
using TotemCli.Injection;

public partial class Program
{
    private readonly IConfiguration _configuration;
    private readonly IServiceCollection _serviceCollection;
    public Program()
    {
        _configuration = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();


        _serviceCollection = new ServiceCollection()
            .AddSingleton(_configuration)
            .AddSingleton<AssetService>()
            .AddSingleton<DestinationService>()
            .AddSingleton<ExperienceService>()
            .AddSingleton<LoadAssetHandler>()
            .AddSingleton<CreateDestinationHandler>()
            .AddSingleton<InteractiveOptionHandler>()
            .AddSingleton<Prompts>();
    }

    static public void Main(string[] args)
    {

        var program = new Program();


        var app = new CommandApp(new TypeRegistrar(program._serviceCollection));
        //var app = new CommandApp();
        app.Configure(config =>
        {
            config.AddCommand<TotemRootCommand>("Start")
                .WithDescription("Interactive console application");
        });

        app.Run(args);



        //var rootCommand = new RootCommand();

        //program._serviceProvider.GetRequiredService<LoadAssetHandler>().AddCommand(rootCommand);
        //program._serviceProvider.GetRequiredService<CreateDestinationHandler>().AddCommand(rootCommand);

        //return await rootCommand.InvokeAsync(args);
    }
}