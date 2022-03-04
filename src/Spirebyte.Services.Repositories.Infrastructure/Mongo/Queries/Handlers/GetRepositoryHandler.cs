using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Spirebyte.Services.Repositories.Application.Repositories.DTO;
using Spirebyte.Services.Repositories.Application.Repositories.Queries;
using Spirebyte.Services.Repositories.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Repositories.Infrastructure.Mongo.Documents.Mappers;

namespace Spirebyte.Services.Repositories.Infrastructure.Mongo.Queries.Handlers;

internal sealed class GetRepositoryHandler : IQueryHandler<GetRepository, RepositoryDto>
{
    private readonly IMongoRepository<RepositoryDocument, string> _repositoryRepository;

    public GetRepositoryHandler(IMongoRepository<RepositoryDocument, string> repositoryRepository)
    {
        _repositoryRepository = repositoryRepository;
    }

    public async Task<RepositoryDto> HandleAsync(GetRepository query, CancellationToken cancellationToken = default)
    {
        var repository = await _repositoryRepository.GetAsync(query.RepositoryId);

        return repository?.AsDto();
    }
}