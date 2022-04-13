using FluentAssertions;
using Spirebyte.Services.Repositories.Core.Entities;
using Spirebyte.Services.Repositories.Tests.Shared.MockData.Entities;
using Xunit;

namespace Spirebyte.Services.Repositories.Tests.Unit.Core.Entities;

public class CommitTests
{
    [Fact]
    public void given_valid_input_commit_should_be_created()
    {
        var fakedCommit = CommitFaker.Instance.Generate();

        var commit = new Commit(fakedCommit.Id, fakedCommit.Sha, fakedCommit.ShortMessage,
            fakedCommit.Message, fakedCommit.AuthorUsername, fakedCommit.CommitterUsername, fakedCommit.CreatedAt);

        commit.Should().NotBeNull();
        commit.Id.Should().Be(fakedCommit.Id);
        commit.Sha.Should().Be(fakedCommit.Sha);
        commit.ShortMessage.Should().Be(fakedCommit.ShortMessage);
        commit.Message.Should().Be(fakedCommit.Message);
        commit.AuthorUsername.Should().Be(fakedCommit.AuthorUsername);
        commit.CommitterUsername.Should().Be(fakedCommit.CommitterUsername);
        commit.CreatedAt.Should().Be(fakedCommit.CreatedAt);
    }
}