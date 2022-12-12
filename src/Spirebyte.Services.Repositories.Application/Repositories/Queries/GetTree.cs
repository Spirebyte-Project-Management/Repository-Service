using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Services.Repositories.Application.Repositories.DTO;

namespace Spirebyte.Services.Repositories.Application.Repositories.Queries;

public class GetTree : IQuery<TreeDto>
{
    public GetTree()
    {
    }

    public GetTree(string repositoryId, string commitSha, string branch, string path)
    {
        RepositoryId = repositoryId;
        CommitSha = commitSha;
        Branch = branch;
        Path = path;
    }

    public string? RepositoryId { get; set; }
    public string? CommitSha { get; set; }
    public string? Branch { get; set; }
    public string? Path { get; set; }
}