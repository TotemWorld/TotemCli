using System.CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using TotemCli.Service;
using System.Security.Cryptography.X509Certificates;

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
            .BuildServiceProvider();
    }


    static async Task<int> Main(string[] args)
    {
        var program = new Program();

        var rootCommand = new RootCommand();

        var pathOption = new Option<string>
        (
            aliases: new[] { "--path", "-t" },
            description: "Provide the file path. You can include a relative path or an absolute path"
        );

        var nameOption = new Option<string>
        (
            aliases: new[] { "--name", "-n" },
            description: "Provide a name for the 3d asset"
        );

        var sendPostCommand = new Command("send-asset", "Send a post request to the Totem api")
        {
            pathOption,
            nameOption
        };

        rootCommand.AddCommand(sendPostCommand);

        sendPostCommand.SetHandler(async (path, name) =>
        {
            await program._serviceProvider.GetRequiredService<AssetService>().SendHttpPostRegisterAsset(path, name);

        }, pathOption, nameOption);

        return await rootCommand.InvokeAsync(args);
    }


}