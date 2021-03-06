using Convey.CQRS.Queries;
using Spirebyte.Services.Repositories.Application.Repositories.DTO;

namespace Spirebyte.Services.Repositories.Application.Repositories.Queries;

public class GetBlob : IQuery<BlobDto>
{
    public GetBlob()
    {
    }

    public GetBlob(string projectId, string repositoryId, string commitSha, string branch, string path)
    {
        ProjectId = projectId;
        RepositoryId = repositoryId;
        CommitSha = commitSha;
        Branch = branch;
        Path = path;
    }

    public string ProjectId { get; set; }
    public string RepositoryId { get; set; }
    public string? CommitSha { get; set; }
    public string? Branch { get; set; }
    public string? Path { get; set; }
}