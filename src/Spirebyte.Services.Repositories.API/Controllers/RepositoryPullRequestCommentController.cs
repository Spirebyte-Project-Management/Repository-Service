using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spirebyte.Framework.API;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Services.Repositories.Application.PullRequests.Commands;
using Spirebyte.Services.Repositories.Application.PullRequests.Services.Interfaces;
using Spirebyte.Services.Repositories.Core.Constants;
using Swashbuckle.AspNetCore.Annotations;

namespace Spirebyte.Services.Repositories.API.Controllers;

[Route("repositories/{repositoryId}/pullRequests/{pullRequestId}/comments")]
public class RepositoryPullRequestCommentController : ApiController
{
    private readonly IDispatcher _dispatcher;
    private readonly IPullRequestActionRequestStorage _pullRequestActionRequestStorage;

    public RepositoryPullRequestCommentController(IDispatcher dispatcher,
        IPullRequestActionRequestStorage pullRequestActionRequestStorage)
    {
        _dispatcher = dispatcher;
        _pullRequestActionRequestStorage = pullRequestActionRequestStorage;
    }

    [HttpPost]
    [Authorize(ApiScopes.RepositoriesPullRequestsWrite)]
    [SwaggerOperation("Create Pull request comment")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAsync(CreatePullRequestComment command, string repositoryId,
        long pullRequestId)
    {
        if (string.IsNullOrEmpty(command.Message)) return BadRequest();

        command.RepositoryId = repositoryId;
        command.PullRequestId = pullRequestId;

        await _dispatcher.SendAsync(command);

        return Created($"repositories/{repositoryId}",
            _pullRequestActionRequestStorage.GetPullRequestAction(command.ReferenceId));
    }
}