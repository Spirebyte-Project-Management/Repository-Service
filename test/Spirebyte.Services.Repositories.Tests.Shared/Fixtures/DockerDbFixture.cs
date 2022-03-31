using System;
using Spirebyte.Services.Repositories.Tests.Shared.Helpers;

namespace Spirebyte.Services.Repositories.Tests.Shared.Fixtures;

public class DockerDbFixture : IDisposable
{
    public DockerDbFixture()
    {
        DockerHelper.DockerComposeUp("integration-test.settings.json");
    }


    public void Dispose()
    {
        DockerHelper.DockerComposeDown();
    }
}