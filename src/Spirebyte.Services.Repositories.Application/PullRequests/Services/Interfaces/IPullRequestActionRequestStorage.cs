using System;
using Spirebyte.Services.Repositories.Core.Entities;

namespace Spirebyte.Services.Repositories.Application.PullRequests.Services.Interfaces;

public interface IPullRequestActionRequestStorage
{
    void SetPullRequestAction(Guid referenceId, PullRequestAction pullRequestAction);
    PullRequestAction GetPullRequestAction(Guid referenceId);
}