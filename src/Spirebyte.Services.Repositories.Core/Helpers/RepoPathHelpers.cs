using System;
using System.IO;
using Spirebyte.Services.Repositories.Core.Entities;

namespace Spirebyte.Services.Repositories.Core.Helpers;

public static class RepoPathHelpers
{
    private static readonly string RepoCacheDirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RepoCache");

    public static string GetCachePathForRepository(Repository repository)
    {
        return GetCachePathForRepositoryId(repository.Id, repository.ReferenceId);
    }
    
    public static string GetCachePathForRepositoryId(string repositoryId, Guid referenceId)
    {
        return Path.Combine(RepoCacheDirPath, repositoryId, referenceId.ToString());
    }

    public static string GetUploadPathForRepo(Repository repository)
    {
        return $"{repository.ProjectId}/{repository.Id}";
    }
}