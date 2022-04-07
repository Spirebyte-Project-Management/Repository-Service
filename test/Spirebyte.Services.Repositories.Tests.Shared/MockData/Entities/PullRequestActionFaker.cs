using Bogus;
using Spirebyte.Services.Repositories.Core.Entities;
using Spirebyte.Services.Repositories.Core.Enums;

namespace Spirebyte.Services.Repositories.Tests.Shared.MockData.Entities;

public class PullRequestActionFaker : Faker<PullRequestActions>
{
    private PullRequestActionFaker()
    {
        CustomInstantiator(_ => new PullRequestActions(default, default, default, default, default));
        RuleFor(r => r.CreatedAt, f => f.Date.Past(1));
        RuleFor(r => r.Type, f => f.Random.Enum<PullRequestActionType>());
        RuleFor(r => r.Message, f => f.Random.Words(7));
        RuleFor(r => r.Commits, f => f.Random.WordsArray(4));
        RuleFor(r => r.UserId, f => f.Random.Guid().ToString());
    }

    public static PullRequestActionFaker Instance => new();
}