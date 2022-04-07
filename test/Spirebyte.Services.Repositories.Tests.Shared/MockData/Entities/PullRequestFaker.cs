using Bogus;
using Spirebyte.Services.Repositories.Core.Entities;
using Spirebyte.Services.Repositories.Core.Enums;

namespace Spirebyte.Services.Repositories.Tests.Shared.MockData.Entities;

public class PullRequestFaker : Faker<PullRequest>
{
    private PullRequestFaker()
    {
        CustomInstantiator(_ => new PullRequest(default, default, default, default, default,default,default, default));
        RuleFor(r => r.Id, f => f.UniqueIndex);
        RuleFor(r => r.Name, f => f.Random.Word());
        RuleFor(r => r.Description, f => f.Random.Words(7));
        RuleFor(r => r.Status, f => f.Random.Enum<PullRequestStatus>());
        RuleFor(r => r.Actions, f => PullRequestActionFaker.Instance.Generate(8));
        RuleFor(r => r.Head, f => f.Random.Word());
        RuleFor(r => r.Branch, f =>  f.Random.Word());
        RuleFor(r => r.CreatedAt, f => f.Date.Past(1));
    }

    public static PullRequestFaker Instance => new();
}