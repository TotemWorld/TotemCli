using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TotemCli.Services;
using TotemCli.Src.Services;
using Spectre.Console.Cli;
using TotemCli.CommandHandler;
using TotemCli.PromptHandler;
using TotemCli.Injection;
using System.Diagnostics;

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
            .AddSingleton<InteractiveOptionHandler>()
            .AddSingleton<Prompts>();

    }
    static public void Main(string[] args)
    {

        var program = new Program();

        var app = new CommandApp(new TypeRegistrar(program._serviceCollection));
            app.Configure(config =>
            {
                config.AddCommand<TotemRootCommand>("Start")
                    .WithDescription("Interactive console application");
        });
        app.Run(args);
    }
}