using System.Linq;
using System.Threading.Tasks;
using Spirebyte.Framework.DAL.MongoDb.Interfaces;
using Spirebyte.Services.Repositories.Core.Entities;
using Spirebyte.Services.Repositories.Core.Repositories;
using Spirebyte.Services.Repositories.Infrastructure.Mongo.Documents;

namespace Spirebyte.Services.Repositories.Infrastructure.Mongo.Repositories;

public class PullRequestRepository : IPullRequestRepository
{
    private readonly IMongoRepository<RepositoryDocument, string> _repository;

    public PullRequestRepository(IMongoRepository<RepositoryDocument, string> repository)
    {
        _repository = repository;
    }

    public async Task<PullRequest> GetAsync(string repositoryId, long pullRequestId)
    {
        var repository = await _repository.GetAsync(x => x.Id == repositoryId);
        var pullRequest = repository.PullRequests.Find(x => x.Id == pullRequestId);

        return pullRequest;
    }

    public async Task<long> GetPullRequestCountOfRepositoryAsync(string repositoryId)
    {
        var repository = await _repository.GetAsync(x => x.Id == repositoryId);

        return repository.PullRequests.Count;
    }

    public async Task<bool> ExistsAsync(string repositoryId, long pullRequestId)
    {
        var repository = await _repository.GetAsync(x => x.Id == repositoryId);

        return repository.PullRequests.Any(c => c.Id == pullRequestId);
    }

    public async Task AddAsync(string repositoryId, PullRequest pullRequest)
    {
        var repository = await _repository.GetAsync(x => x.Id == repositoryId);
        repository.PullRequests.Add(pullRequest);
        await _repository.UpdateAsync(repository);
    }

    public async Task UpdateAsync(string repositoryId, PullRequest pullRequest)
    {
        var repository = await _repository.GetAsync(x => x.Id == repositoryId);
        var index = repository.PullRequests.FindIndex(x => x.Id == pullRequest.Id);
        if (index > -1) repository.PullRequests[index] = pullRequest;

        await _repository.UpdateAsync(repository);
    }

    public async Task DeleteAsync(string repositoryId, long pullRequestId)
    {
        var repository = await _repository.GetAsync(x => x.Id == repositoryId);
        var pullRequest = repository.PullRequests.Find(x => x.Id == pullRequestId);

        repository.PullRequests.Remove(pullRequest);
        await _repository.UpdateAsync(repository);
    }
}