using System.Threading.Tasks;
using Convey.WebApi;
using Convey.WebApi.CQRS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spirebyte.Services.Repositories.API.Controllers.Base;
using Spirebyte.Services.Repositories.Application.Branches.Commands;
using Spirebyte.Services.Repositories.Application.Branches.Services.Interfaces;
using Spirebyte.Services.Repositories.Core.Constants;
using Swashbuckle.AspNetCore.Annotations;

namespace Spirebyte.Services.Repositories.API.Controllers;

[Route("repositories/{repositoryId}/branches")]
public class RepositoryBranchController : BaseController
{
    private readonly IBranchRequestStorage _branchRequestStorage;
    private readonly IDispatcher _dispatcher;

    public RepositoryBranchController(IDispatcher dispatcher, IBranchRequestStorage branchRequestStorage)
    {
        _dispatcher = dispatcher;
        _branchRequestStorage = branchRequestStorage;
    }

    [HttpPost]
    [Authorize(ApiScopes.Write)]
    [SwaggerOperation("Create Branch")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateAsync(CreateBranch command, string repositoryId)
    {
        if (string.IsNullOrEmpty(command.Title)) return BadRequest();

        await _dispatcher.SendAsync(command.Bind(q => q.RepositoryId, repositoryId));

        return Created($"repositories/{repositoryId}", _branchRequestStorage.GetBranch(command.ReferenceId));
    }

    [HttpDelete("{*branchId}")]
    [Authorize(ApiScopes.Delete)]
    [SwaggerOperation("Delete Branch")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteAsync(string repositoryId, string branchId)
    {
        await _dispatcher.SendAsync(new DeleteBranch(branchId, repositoryId));

        return Ok();
    }
}