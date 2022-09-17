using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Services.Repositories.API.Controllers.Base;
using Spirebyte.Services.Repositories.Application.PullRequests.Commands;
using Spirebyte.Services.Repositories.Application.PullRequests.Services.Interfaces;
using Spirebyte.Services.Repositories.Core.Constants;
using Swashbuckle.AspNetCore.Annotations;

namespace Spirebyte.Services.Repositories.API.Controllers;

[Route("repositories/{repositoryId}/pullRequests")]
public class RepositoryPullRequestsController : BaseController
{
    private readonly IDispatcher _dispatcher;
    private readonly IPullRequestRequestStorage _pullRequestRequestStorage;

    public RepositoryPullRequestsController(IDispatcher dispatcher,
        IPullRequestRequestStorage pullRequestRequestStorage)
    {
        _dispatcher = dispatcher;
        _pullRequestRequestStorage = pullRequestRequestStorage;
    }

    [HttpPost]
    [Authorize(ApiScopes.Write)]
    [SwaggerOperation("Create Pull request")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateAsync(CreatePullRequest command, string repositoryId)
    {
        if (string.IsNullOrEmpty(command.Branch) || string.IsNullOrEmpty(command.Head)) return BadRequest();

        command.RepositoryId = repositoryId;        
        
        await _dispatcher.SendAsync(command);

        return Created($"repositories/{repositoryId}", _pullRequestRequestStorage.GetPullRequest(command.ReferenceId));
    }
}