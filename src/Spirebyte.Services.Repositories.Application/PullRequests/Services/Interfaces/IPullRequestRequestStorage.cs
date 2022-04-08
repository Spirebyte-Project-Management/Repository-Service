using System;
using Spirebyte.Services.Repositories.Core.Entities;

namespace Spirebyte.Services.Repositories.Application.PullRequests.Services.Interfaces;

public interface IPullRequestRequestStorage
{
    void SetPullRequest(Guid referenceId, PullRequest pullRequest);
    PullRequest GetPullRequest(Guid referenceId);
}