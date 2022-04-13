using Bogus;
using Spirebyte.Services.Repositories.Core.Entities;

namespace Spirebyte.Services.Repositories.Tests.Shared.MockData.Entities;

public sealed class ProjectFaker : Faker<Project>
{
    private ProjectFaker()
    {
        CustomInstantiator(_ => new Project(default));
        RuleFor(r => r.Id, f => f.Random.Word());
    }

    public static ProjectFaker Instance => new();
}