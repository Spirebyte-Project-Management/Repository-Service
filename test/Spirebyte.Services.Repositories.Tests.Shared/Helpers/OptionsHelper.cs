using Microsoft.Extensions.Configuration;
using Spirebyte.Services.Repositories.Tests.Shared.Const;

namespace Spirebyte.Services.Repositories.Tests.Shared.Helpers;

public class OptionsHelper
{
    public static TSettings GetOptions<TSettings>(string section, string settingsFileName = null)
        where TSettings : class, new()
    {
        settingsFileName ??= AppsettingsConst.IntegrationTest;
        var configuration = new TSettings();

        GetConfigurationRoot(settingsFileName)
            
            .GetSection(section)
            .Bind(configuration);

        return configuration;
    }

    private static IConfigurationRoot GetConfigurationRoot(string settingsFileName)
    {
        return new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile(settingsFileName, true)
            .AddEnvironmentVariables()
            .Build();
    }
}