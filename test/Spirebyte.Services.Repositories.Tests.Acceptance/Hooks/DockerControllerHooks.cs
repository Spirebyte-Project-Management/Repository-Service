﻿using System.Net;
using BoDi;
using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;
using Microsoft.Extensions.Configuration;

namespace Spirebyte.Services.Repositories.Tests.Acceptance.Hooks;

[Binding]
public class DockerControllerHooks
{
    private static ICompositeService _compositeService;
    private IObjectContainer _objectContainer;

    public DockerControllerHooks(IObjectContainer objectContainer)
    {
        _objectContainer = objectContainer;
    }

    [BeforeTestRun]
    public static void DockerComposeUp()
    {
        var config = LoadConfiguration();

        var dockerComposeFileName = config["DockerComposeFileName"];
        var dockerComposePath = GetDockerComposeLocation(dockerComposeFileName);

        var confirmationUrl = config["Spirebyte.Services.Repositories.API:TestingAddress"];
        _compositeService = new Builder()
            .UseContainer()
            .UseCompose()
            .FromFile(dockerComposePath)
            .RemoveOrphans()
            .WaitForHttp("webapi", $"{confirmationUrl}/ping",
                continuation: (response, _) => response.Code != HttpStatusCode.OK ? 2000 : 0)
            .Build()
            .Start();
    }
    
    [AfterTestRun]
    public static void DockerComposeDown()
    {
        _compositeService.Stop();
        _compositeService.Dispose();
    }

    [BeforeScenario]
    public void AddHttpClient()
    {
        var config = LoadConfiguration();
        var httpClient = new HttpClient
        {
            BaseAddress = new Uri(config["Spirebyte.Services.Repositories.API:TestingAddress"])
        };
        _objectContainer.RegisterInstanceAs(httpClient);
    }

    private static IConfiguration LoadConfiguration()
    {
        return new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
    }

    private static string GetDockerComposeLocation(string dockerComposeFileName)
    {
        var directory = Directory.GetCurrentDirectory();
        while (!Directory.EnumerateFiles(directory, "*.yml").Any(s => s.EndsWith(dockerComposeFileName)))
        {
            directory = directory.Substring(0, directory.LastIndexOf(Path.DirectorySeparatorChar));
        }

        return Path.Combine(directory, dockerComposeFileName);
    }
}