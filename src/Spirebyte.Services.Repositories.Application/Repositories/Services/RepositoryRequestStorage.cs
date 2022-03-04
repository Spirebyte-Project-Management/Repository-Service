using System;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using Spirebyte.Services.Repositories.Application.Repositories.DTO;
using Spirebyte.Services.Repositories.Application.Repositories.Services.Interfaces;
using Spirebyte.Services.Repositories.Core.Entities;

namespace Spirebyte.Services.Repositories.Application.Repositories.Services;

public class RepositoryRequestStorage : IRepositoryRequestStorage
{
    private readonly IMemoryCache _cache;

    public RepositoryRequestStorage(IMemoryCache cache)
    {
        _cache = cache;
    }

    public void SetRepository(Guid referenceId, Repository repository)
    {
        var issueDto = new RepositoryDto
        {
            Id = repository.Id,
            Title = repository.Title,
            Description = repository.Description,
            ProjectId = repository.ProjectId,
            CreatedAt = repository.CreatedAt,
        };

        _cache.Set(GetKey(referenceId), issueDto, TimeSpan.FromSeconds(5));
    }

    public RepositoryDto GetRepository(Guid referenceId)
    {
        return _cache.Get<RepositoryDto>(GetKey(referenceId));
    }

    private static string GetKey(Guid commandId)
    {
        return $"Repository:{commandId:N}";
    }
}