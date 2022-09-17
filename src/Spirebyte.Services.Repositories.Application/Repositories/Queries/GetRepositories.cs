using System.Collections.Generic;
using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Services.Repositories.Application.Repositories.DTO;

namespace Spirebyte.Services.Repositories.Application.Repositories.Queries;

public class GetRepositories : IQuery<IEnumerable<RepositoryDto>>
{
    public GetRepositories()
    {
    }

    public GetRepositories(string? projectId = null)
    {
        ProjectId = projectId;
    }

    public string? ProjectId { get; set; }
}