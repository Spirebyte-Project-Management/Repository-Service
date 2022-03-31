using System.IO;
using System.Linq;
using System.Net;
using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;
using Microsoft.Extensions.Configuration;

namespace Spirebyte.Services.Repositories.Tests.Shared.Helpers;

public class DockerHelper
{
    private static ICompositeService _compositeService;

    
    public static void DockerComposeUp(string settingsFile)
    {
        var config = LoadConfiguration(settingsFile);

        var dockerComposeFileName = config["DockerComposeFileName"];
        var dockerComposePath = GetDockerComposeLocation(dockerComposeFileName);

        _compositeService = new Builder()
            .UseContainer()
            .UseCompose()
            .FromFile(dockerComposePath)
            .ForceBuild()
            .RemoveOrphans()
            .ForceRecreate()
            .Build()
            .Start();
    }

    public static void DockerComposeDown()
    {
        _compositeService.Stop();
        _compositeService.Dispose();
    }
    
    private static IConfiguration LoadConfiguration(string settingFile)
    {
        return new ConfigurationBuilder()
            .AddJsonFile(settingFile)
            .Build();
    }

    private static string GetDockerComposeLocation(string dockerComposeFileName)
    {
        var directory = Directory.GetCurrentDirectory();
        while (!Directory.EnumerateFiles(directory, "*.yml").Any(s => s.EndsWith(dockerComposeFileName)))
            directory = directory.Substring(0, directory.LastIndexOf(Path.DirectorySeparatorChar));

        return Path.Combine(directory, dockerComposeFileName);
    }
}