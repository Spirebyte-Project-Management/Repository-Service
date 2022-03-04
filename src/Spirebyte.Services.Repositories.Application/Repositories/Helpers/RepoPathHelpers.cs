using System;
using System.IO;
using Spirebyte.Services.Repositories.Core.Entities;

namespace Spirebyte.Services.Repositories.Application.Repositories.Helpers;

public static class RepoPathHelpers
{
    private static readonly string RepoCacheDirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RepoCache");

    public static string GetCachePathForRepository(Repository repository)
    {
        return GetCachePathForRepositoryId(repository.Id);
    }
    
    public static string GetCachePathForRepositoryId(string repositoryId)
    {
        return Path.Combine(RepoCacheDirPath, repositoryId);
    }
}