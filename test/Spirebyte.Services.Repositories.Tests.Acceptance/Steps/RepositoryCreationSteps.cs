using FluentAssertions;
using RestSharp;
using Spirebyte.Services.Repositories.Application.Repositories.Commands;
using Spirebyte.Services.Repositories.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Repositories.Tests.Acceptance.API;
using Spirebyte.Services.Repositories.Tests.Acceptance.Drivers;
using TechTalk.SpecFlow.Assist;

namespace Spirebyte.Services.Repositories.Tests.Acceptance.Steps;

[Binding]
public class RepositoryCreationSteps
{
    private readonly RepositoryApi _repositoryApi;
    private readonly MongoDbDriver _mongoDbDriver;
    private readonly JsonDriver _jsonDriver;
    private readonly ScenarioContext _scenarioContext;

    private static readonly Guid UserId = Guid.NewGuid();
    
    public RepositoryCreationSteps(RepositoryApi repositoryApi, MongoDbDriver mongoDbDriver, JsonDriver jsonDriver, ScenarioContext scenarioContext)
    {
        _repositoryApi = repositoryApi;
        _mongoDbDriver = mongoDbDriver;
        _jsonDriver = jsonDriver;
        _scenarioContext = scenarioContext;
    }
    
    [Given(@"the project ([a-zA-Z]+) exists")]
    public async Task GivenTheProjectExists(string projectId)
    {
        if (!await _mongoDbDriver.ProjectRepository.ExistsAsync(p => p.Id == projectId))
        {
            await _mongoDbDriver.ProjectRepository.AddAsync(new ProjectDocument() { Id = projectId });
        }
    }

    [When(@"I try to create a repository with the following details")]
    public async Task WhenITryToCreateARepositoryWithTheFollowingDetails(Table table)
    {
        var createRepositoryCommands = table.CreateSet<CreateRepository>();
        var createdRepositories = new List<RestResponse>();
        foreach (var repositoryCommand in createRepositoryCommands)
        {
            var response = await _repositoryApi.CreateAsync(repositoryCommand, UserId);
            createdRepositories.Add(response);
        }
        _scenarioContext.Add("CreatedRepositories", createdRepositories);
    }

    [Then(@"the repository is created successfully")]
    public async Task ThenTheRepositoryIsCreatedSuccessfully()
    {
        var createdRepositoryResponses = _scenarioContext.Get<List<RestResponse>>("CreatedRepositories");
        foreach (var createdRepositoryResponse in createdRepositoryResponses)
        {
            createdRepositoryResponse.IsSuccessful.Should().BeTrue();
        }
    }

    [Given(@"the project ([a-zA-Z]+) does not exists")]
    public async Task GivenTheProjectDoesNotExists(string projectId)
    {
        if (await _mongoDbDriver.ProjectRepository.ExistsAsync(p => p.Id == projectId))
        {
            await _mongoDbDriver.ProjectRepository.DeleteAsync(projectId);
        }
    }

    [Then(@"the repository fails to be created")]
    public void ThenTheRepositoryFailsToBeCreated()
    {
        var createdRepositoryResponses = _scenarioContext.Get<List<RestResponse>>("CreatedRepositories");
        foreach (var createdRepositoryResponse in createdRepositoryResponses)
        {
            createdRepositoryResponse.IsSuccessful.Should().BeFalse();
        }
    }
}