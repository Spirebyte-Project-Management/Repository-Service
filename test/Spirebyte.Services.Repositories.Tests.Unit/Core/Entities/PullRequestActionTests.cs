using FluentAssertions;
using Spirebyte.Services.Repositories.Core.Entities;
using Spirebyte.Services.Repositories.Tests.Shared.MockData.Entities;
using Xunit;

namespace Spirebyte.Services.Repositories.Tests.Unit.Core.Entities;

public class PullRequestActionTests
{
    [Fact]
    public void given_valid_input_pull_request_action_should_be_created()
    {
        var fakedPullRequestAction = PullRequestActionFaker.Instance.Generate();

        var pullRequestAction = new PullRequestAction(fakedPullRequestAction.CreatedAt, fakedPullRequestAction.Type,
            fakedPullRequestAction.Message, fakedPullRequestAction.Commits, fakedPullRequestAction.UserId);

        pullRequestAction.Should().NotBeNull();
        pullRequestAction.CreatedAt.Should().Be(fakedPullRequestAction.CreatedAt);
        pullRequestAction.Type.Should().Be(fakedPullRequestAction.Type);
        pullRequestAction.Message.Should().Be(fakedPullRequestAction.Message);
        pullRequestAction.Commits.Should().Contain(fakedPullRequestAction.Commits);
        pullRequestAction.UserId.Should().Be(fakedPullRequestAction.UserId);
    }
}