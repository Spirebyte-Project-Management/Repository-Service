using FluentAssertions;
using Spirebyte.Services.Repositories.Core.Entities;
using Spirebyte.Services.Repositories.Tests.Shared.MockData.Entities;
using Xunit;

namespace Spirebyte.Services.Repositories.Tests.Unit.Core.Entities;

public class PullRequestTests
{
    [Fact]
    public void given_valid_input_pull_request_should_be_created()
    {
        var fakedPullRequest = PullRequestFaker.Instance.Generate();

        var pullRequest = new PullRequest(fakedPullRequest.Id, fakedPullRequest.Name, fakedPullRequest.Description,
            fakedPullRequest.Status, fakedPullRequest.Actions, fakedPullRequest.Head, fakedPullRequest.Branch,
            fakedPullRequest.UserId, fakedPullRequest.CreatedAt, fakedPullRequest.UpdatedAt);

        pullRequest.Should().NotBeNull();
        pullRequest.Id.Should().Be(fakedPullRequest.Id);
        pullRequest.Name.Should().Be(fakedPullRequest.Name);
        pullRequest.Description.Should().Be(fakedPullRequest.Description);
        pullRequest.Status.Should().Be(fakedPullRequest.Status);
        pullRequest.Actions.Should().Contain(fakedPullRequest.Actions);
        pullRequest.Head.Should().Be(fakedPullRequest.Head);
        pullRequest.Branch.Should().Be(fakedPullRequest.Branch);
        pullRequest.UserId.Should().Be(fakedPullRequest.UserId);
        pullRequest.CreatedAt.Should().Be(fakedPullRequest.CreatedAt);
        pullRequest.UpdatedAt.Should().Be(fakedPullRequest.UpdatedAt);
    }

    [Fact]
    public void pull_request_add_action_should_add_action()
    {
        var fakedPullRequest = PullRequestFaker.Instance.Generate();
        var fakedPullRequestAction = PullRequestActionFaker.Instance.Generate();

        fakedPullRequest.AddAction(fakedPullRequestAction);

        fakedPullRequest.Actions.Should().Contain(fakedPullRequestAction);
    }
}