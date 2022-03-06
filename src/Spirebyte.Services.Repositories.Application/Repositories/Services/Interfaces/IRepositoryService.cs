using System.Threading.Tasks;
using Spirebyte.Services.Repositories.Core.Entities;

namespace Spirebyte.Services.Repositories.Application.Repositories.Services.Interfaces;

public interface IRepositoryService
{
    Task EnsureLatestRepositoryIsCached(Repository repository);
    Task<Repository> UploadRepoChanges(Repository repository);
}