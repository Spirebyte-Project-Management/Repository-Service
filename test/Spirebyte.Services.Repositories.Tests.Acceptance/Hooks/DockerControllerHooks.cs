using Spirebyte.Services.Repositories.Tests.Shared.Helpers;

namespace Spirebyte.Services.Repositories.Tests.Acceptance.Hooks;

[Binding]
public class DockerControllerHooks
{
    [BeforeTestRun]
    public static void DockerComposeUp()
    {
        DockerHelper.DockerComposeUp(SettingsConst.AcceptanceTestsSettings);
    }

    [AfterTestRun]
    public static void DockerComposeDown()
    {
        DockerHelper.DockerComposeDown();
    }
}