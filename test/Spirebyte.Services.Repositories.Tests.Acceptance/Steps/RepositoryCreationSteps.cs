using Spirebyte.Services.Repositories.Application.Repositories.Commands;
using TechTalk.SpecFlow.Assist;

namespace Spirebyte.Services.Repositories.Tests.Acceptance.Steps;

[Binding]
public class RepositoryCreationSteps
{
    [Given(@"the project (.*) exists")]
    public void GivenTheProjectExists()
    {
        ScenarioContext.StepIsPending();
    }

    [When(@"I try to create a repository with the following details")]
    public void WhenITryToCreateARepositoryWithTheFollowingDetails(Table table)
    {
        var createRepositoryCommands = table.CreateSet<CreateRepository>();
        
    }

    [Then(@"the repository is created successfully")]
    public void ThenTheRepositoryIsCreatedSuccessfully()
    {
        ScenarioContext.StepIsPending();
    }

    [Given(@"the project (.*) does not exists")]
    public void GivenTheProjectDoesNotExists()
    {
        ScenarioContext.StepIsPending();
    }

    [Then(@"the repository fails to be created")]
    public void ThenTheRepositoryFailsToBeCreated()
    {
        ScenarioContext.StepIsPending();
    }
}