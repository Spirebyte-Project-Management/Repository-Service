using System;
using Microsoft.Extensions.Caching.Memory;
using Spirebyte.Services.Repositories.Application.PullRequests.Services.Interfaces;
using Spirebyte.Services.Repositories.Core.Entities;

namespace Spirebyte.Services.Repositories.Application.PullRequests.Services;

public class PullRequestActionRequestStorage : IPullRequestActionRequestStorage
{
    private readonly IMemoryCache _cache;

    public PullRequestActionRequestStorage(IMemoryCache cache)
    {
        _cache = cache;
    }

    public void SetPullRequestAction(Guid referenceId, PullRequestAction pullRequestAction)
    {
        _cache.Set(GetKey(referenceId), pullRequestAction, TimeSpan.FromSeconds(5));
    }

    public PullRequestAction GetPullRequestAction(Guid referenceId)
    {
        return _cache.Get<PullRequestAction>(GetKey(referenceId));
    }

    private static string GetKey(Guid commandId)
    {
        return $"PullRequestAction:{commandId:N}";
    }
}