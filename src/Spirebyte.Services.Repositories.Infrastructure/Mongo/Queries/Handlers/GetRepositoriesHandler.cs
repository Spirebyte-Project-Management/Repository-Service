using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Spirebyte.Services.Repositories.Application.Projects.Exceptions;
using Spirebyte.Services.Repositories.Application.Repositories.DTO;
using Spirebyte.Services.Repositories.Application.Repositories.Queries;
using Spirebyte.Services.Repositories.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Repositories.Infrastructure.Mongo.Documents.Mappers;

namespace Spirebyte.Services.Repositories.Infrastructure.Mongo.Queries.Handlers;

internal sealed class GetRepositoriesHandler : IQueryHandler<GetRepositories, IEnumerable<RepositoryDto>>
{
    private readonly IMongoRepository<ProjectDocument, string> _projectRepository;
    private readonly IMongoRepository<RepositoryDocument, string> _repositoryRepository;

    public GetRepositoriesHandler(IMongoRepository<RepositoryDocument, string> repositoryRepository,
        IMongoRepository<ProjectDocument, string> projectRepository)
    {
        _repositoryRepository = repositoryRepository;
        _projectRepository = projectRepository;
    }

    public async Task<IEnumerable<RepositoryDto>> HandleAsync(GetRepositories query,
        CancellationToken cancellationToken = default)
    {
        var documents = _repositoryRepository.Collection.AsQueryable();

        if (string.IsNullOrWhiteSpace(query.ProjectId)) return new List<RepositoryDto>();

        if (!await _projectRepository.ExistsAsync(p => p.Id == query.ProjectId))
            throw new ProjectNotFoundException(query.ProjectId);

        var repositoriesWithProject = documents.Where(p => p.ProjectId == query.ProjectId).Select(p => p.AsDto());

        return await repositoriesWithProject.ToListAsync(cancellationToken: cancellationToken);
    }
}