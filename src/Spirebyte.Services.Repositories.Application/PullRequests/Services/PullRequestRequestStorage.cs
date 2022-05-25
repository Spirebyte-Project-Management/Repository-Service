using System;
using Microsoft.Extensions.Caching.Memory;
using Spirebyte.Services.Repositories.Application.PullRequests.Services.Interfaces;
using Spirebyte.Services.Repositories.Core.Entities;

namespace Spirebyte.Services.Repositories.Application.PullRequests.Services;

public class PullRequestRequestStorage : IPullRequestRequestStorage
{
    private readonly IMemoryCache _cache;

    public PullRequestRequestStorage(IMemoryCache cache)
    {
        _cache = cache;
    }

    public void SetPullRequest(Guid referenceId, PullRequest pullRequest)
    {
        _cache.Set(GetKey(referenceId), pullRequest, TimeSpan.FromSeconds(5));
    }

    public PullRequest GetPullRequest(Guid referenceId)
    {
        return _cache.Get<PullRequest>(GetKey(referenceId));
    }

    private static string GetKey(Guid commandId)
    {
        return $"PullRequest:{commandId:N}";
    }
}