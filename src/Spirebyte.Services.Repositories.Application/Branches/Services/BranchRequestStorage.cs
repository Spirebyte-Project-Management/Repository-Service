using System;
using Microsoft.Extensions.Caching.Memory;
using Spirebyte.Services.Repositories.Application.Branches.Services.Interfaces;
using Spirebyte.Services.Repositories.Core.Entities;

namespace Spirebyte.Services.Repositories.Application.Branches.Services;

public class BranchRequestStorage : IBranchRequestStorage
{
    private readonly IMemoryCache _cache;

    public BranchRequestStorage(IMemoryCache cache)
    {
        _cache = cache;
    }

    public void SetBranch(Guid referenceId, Branch branch)
    {
        _cache.Set(GetKey(referenceId), branch, TimeSpan.FromSeconds(5));
    }

    public Branch GetBranch(Guid referenceId)
    {
        return _cache.Get<Branch>(GetKey(referenceId));
    }

    private static string GetKey(Guid commandId)
    {
        return $"Branch:{commandId:N}";
    }
}