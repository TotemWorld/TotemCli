using System;
using System.CommandLine;
using TotemCli.Services;

namespace TotemCli.CommandHandlers
{
    public class LoadAssetHandler
    {
        private readonly AssetService _assetService;

        public LoadAssetHandler
        (
            AssetService assetService
        )
        {
            _assetService = assetService;
        }

        public void AddCommand(RootCommand rootCommand)
        {
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

            var experienceOption = new Option<string>
            (
                aliases: new[] { "--experience", "-e" },
                description: "Provide an experience)"
            );

            var longitudeOption = new Option<string>
            (
                aliases: new[] { "--longitude", "-lg" },
                description: "Provide a longitude"
            );

            var latitudeOption = new Option<string>
            (
                aliases: new[] { "--latitude", "-lt" },
                description: "Provide a latitude"
            );

            var loadAssetCommand = new Command("load-asset", "Send a post request to the Totem api")
            {
                pathOption,
                nameOption,
                experienceOption,
                longitudeOption,
                latitudeOption
            };

            rootCommand.AddCommand( loadAssetCommand );

            loadAssetCommand.SetHandler(_assetService.SendHttpPostRegisterAsset, pathOption, nameOption, experienceOption, longitudeOption, latitudeOption);
        }
    }
}


