using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Services.Repositories.Application.Repositories.DTO;

namespace Spirebyte.Services.Repositories.Application.Repositories.Queries;

public class GetRepository : IQuery<RepositoryDto>
{
    public GetRepository()
    {
    }

    public GetRepository(string repositoryId)
    {
        RepositoryId = repositoryId;
    }

    public string RepositoryId { get; set; }
}