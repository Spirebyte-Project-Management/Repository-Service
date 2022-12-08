using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spirebyte.Framework.API;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Services.Repositories.Application.Branches.Commands;
using Spirebyte.Services.Repositories.Application.Branches.Services.Interfaces;
using Spirebyte.Services.Repositories.Core.Constants;
using Swashbuckle.AspNetCore.Annotations;

namespace Spirebyte.Services.Repositories.API.Controllers;

[Route("repositories/{repositoryId}/branches")]
public class RepositoryBranchController : ApiController
{
    private readonly IBranchRequestStorage _branchRequestStorage;
    private readonly IDispatcher _dispatcher;

    public RepositoryBranchController(IDispatcher dispatcher, IBranchRequestStorage branchRequestStorage)
    {
        _dispatcher = dispatcher;
        _branchRequestStorage = branchRequestStorage;
    }

    [HttpPost]
    [Authorize(ApiScopes.RepositoriesWrite)]
    [SwaggerOperation("Create Branch")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAsync(CreateBranch command, string repositoryId)
    {
        if (string.IsNullOrEmpty(command.Title)) return BadRequest();

        command.RepositoryId = repositoryId;
        
        await _dispatcher.SendAsync(command);

        return Created($"repositories/{repositoryId}", _branchRequestStorage.GetBranch(command.ReferenceId));
    }

    [HttpDelete("{*branchId}")]
    [Authorize(ApiScopes.RepositoriesDelete)]
    [SwaggerOperation("Delete Branch")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteAsync(string repositoryId, string branchId)
    {
        await _dispatcher.SendAsync(new DeleteBranch(branchId, repositoryId));

        return Ok();
    }
}