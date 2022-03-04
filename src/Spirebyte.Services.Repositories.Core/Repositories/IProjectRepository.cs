using System.Threading.Tasks;
using Spirebyte.Services.Repositories.Core.Entities;

namespace Spirebyte.Services.Repositories.Core.Repositories;

public interface IProjectRepository
{
    Task<Project> GetAsync(string id);
    Task<bool> ExistsAsync(string id);
    Task AddAsync(Project project);
}