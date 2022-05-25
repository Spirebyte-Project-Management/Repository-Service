using System.Threading.Tasks;
using Convey.WebApi;
using Convey.WebApi.CQRS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spirebyte.Services.Repositories.API.Controllers.Base;
using Spirebyte.Services.Repositories.Application.PullRequests.Commands;
using Spirebyte.Services.Repositories.Application.PullRequests.Commands.Handler;
using Spirebyte.Services.Repositories.Application.PullRequests.Services.Interfaces;
using Spirebyte.Services.Repositories.Core.Constants;
using Swashbuckle.AspNetCore.Annotations;

namespace Spirebyte.Services.Repositories.API.Controllers;

[Route("repositories/{repositoryId}/pullRequests/{pullRequestId}/comments")]
public class RepositoryPullRequestCommentController : BaseController
{
    private readonly IDispatcher _dispatcher;
    private readonly IPullRequestActionRequestStorage _pullRequestActionRequestStorage;

    public RepositoryPullRequestCommentController(IDispatcher dispatcher, IPullRequestActionRequestStorage pullRequestActionRequestStorage)
    {
        _dispatcher = dispatcher;
        _pullRequestActionRequestStorage = pullRequestActionRequestStorage;
    }

    [HttpPost]
    [Authorize(ApiScopes.Write)]
    [SwaggerOperation("Create Pull request comment")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateAsync(CreatePullRequestComment command, string repositoryId, long pullRequestId)
    {
        if (string.IsNullOrEmpty(command.Message))
        {
            return BadRequest();
        }

        command.Bind(q => q.RepositoryId, repositoryId);
        command.Bind(q => q.PullRequestId, pullRequestId);
        
        await _dispatcher.SendAsync(command);
        
        return Created($"repositories/{repositoryId}", _pullRequestActionRequestStorage.GetPullRequestAction(command.ReferenceId));
    }
}