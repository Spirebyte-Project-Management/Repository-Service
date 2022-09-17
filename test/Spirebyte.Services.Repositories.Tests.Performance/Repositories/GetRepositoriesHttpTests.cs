using System.Text.Json;
using FluentAssertions;
using NBomber.Contracts;
using NBomber.CSharp;
using NBomber.Plugins.Http.CSharp;
using NBomber.Plugins.Network.Ping;
using Spirebyte.Framework.Tests.Shared.Const;
using Spirebyte.Framework.Tests.Shared.Fixtures;
using Spirebyte.Framework.Tests.Shared.Helpers;
using Spirebyte.Services.Repositories.Application.Repositories.DTO;
using Xunit;

namespace Spirebyte.Services.Repositories.Tests.Performance.Repositories;

[Collection("Spirebyte collection")]
public class GetRepositoriesHttpTests
{
    private readonly PerformanceFixture _performanceFixture;

    public GetRepositoriesHttpTests(PerformanceFixture performanceFixture)
    {
        _performanceFixture = performanceFixture;
    }

    private Scenario BuildScenario()
    {
        var authToken = AuthHelper.GenerateJwt(_performanceFixture.PerformanceOptions.UserId);
        var httpFactory = HttpClientFactory.Create();
        var step = Step.Create("get_repositories", httpFactory, async context =>
        {
            var url =
                $"{_performanceFixture.PerformanceOptions.BaseUrl}/repositories/?projectId={_performanceFixture.PerformanceOptions.ProjectId}";

            var request = Http.CreateRequest("GET", url)
                .WithHeader("Authorization", $"bearer {authToken}")
                .WithCheck(async response =>
                {
                    var json = await response.Content.ReadAsStringAsync();

                    // parse JSON
                    var repositories = JsonSerializer.Deserialize<RepositoryDto[]>(json);

                    return response.IsSuccessStatusCode && repositories.Length > 0
                        ? Response.Ok(repositories,
                            (int)response.StatusCode) // we pass user object response to the next step
                        : Response.Fail("not found repositories");
                });

            return await Http.Send(request, context);
        }, TimeSpan.FromSeconds(10));

        return ScenarioBuilder.CreateScenario("get_repositories_test", step);
    }

    [Fact]
    public void Get_Repositories_Should_Have_A_Latency_Less_Than_1000()
    {
        var scenario = BuildScenario();

        var pingPlugin = new PingPlugin();

        var nodeStats = NBomberRunner
            .RegisterScenarios(scenario)
            .WithWorkerPlugins(pingPlugin)
            .LoadConfig(AppsettingsConst.PerformanceTest)
            .LoadInfraConfig(AppsettingsConst.PerformanceTest)
            .Run();

        var stepStats = nodeStats.ScenarioStats[0].StepStats;

        stepStats.Should().AllSatisfy(x =>
        {
            x.Ok.Request.RPS.Should().BeGreaterThan(0);
            x.Ok.Latency.Percent75.Should().BeLessThan(1000);
        });
    }
}