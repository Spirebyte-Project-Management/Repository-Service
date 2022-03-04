using System.Threading.Tasks;
using Convey.WebApi.CQRS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spirebyte.Services.Repositories.API.Controllers.Base;
using Spirebyte.Services.Repositories.Application.Repositories.Commands;
using Spirebyte.Services.Repositories.Application.Repositories.DTO;
using Spirebyte.Services.Repositories.Application.Repositories.Queries;
using Spirebyte.Services.Repositories.Application.Repositories.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace Spirebyte.Services.Repositories.API.Controllers;

[Authorize]
public class RepositoriesController : BaseController
{
    private readonly IDispatcher _dispatcher;
    private readonly IRepositoryRequestStorage _repositoryRequestStorage;

    public RepositoriesController(IDispatcher dispatcher, IRepositoryRequestStorage repositoryRequestStorage)
    {
        _dispatcher = dispatcher;
        _repositoryRequestStorage = repositoryRequestStorage;
    }

    [HttpGet]
    [SwaggerOperation("Browse Repositories")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<RepositoryDto>> BrowseAsync([FromQuery] GetRepositories query)
    {
        return Ok(await _dispatcher.QueryAsync(query));
    }

    [HttpGet("{repositoryId}")]
    [SwaggerOperation("Get Repository")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<RepositoryDto>> GetAsync(string repositoryId)
    {
        return OkOrNotFound(await _dispatcher.QueryAsync(new GetRepository(repositoryId)));
    }

    [HttpPost]
    [SwaggerOperation("Create Repository")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateRepository(CreateRepository command)
    {
        await _dispatcher.SendAsync(command);
        var repository = _repositoryRequestStorage.GetRepository(command.ReferenceId);
        return Created($"Repositories/{repository.Id}", repository);
    }


    [HttpPut("{repositoryId}")]
    [SwaggerOperation("Update Repository")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdatePermissionScheme(string repositoryId, UpdateRepository command)
    {
        if (!command.Id.Equals(repositoryId)) return NotFound();

        await _dispatcher.SendAsync(command);
        return Ok();
    }

    [HttpDelete("{repositoryId}")]
    [SwaggerOperation("Delete Repository")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteRepository(string repositoryId)
    {
        await _dispatcher.SendAsync(new DeleteRepository(repositoryId));
        return Ok();
    }
}