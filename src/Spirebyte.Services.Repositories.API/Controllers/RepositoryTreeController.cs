using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spirebyte.Framework.API;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Services.Repositories.Application.Repositories.DTO;
using Spirebyte.Services.Repositories.Application.Repositories.Queries;
using Spirebyte.Services.Repositories.Core.Constants;
using Swashbuckle.AspNetCore.Annotations;

namespace Spirebyte.Services.Repositories.API.Controllers;

[Route("repositories/{repositoryId}/tree")]
public class RepositoryTreeController : ApiController
{
    private readonly IDispatcher _dispatcher;

    public RepositoryTreeController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    [HttpGet]
    [Authorize(ApiScopes.RepositoriesRead)]
    [SwaggerOperation("Browse Repository Tree")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<TreeDto>> BrowseAsync(string repositoryId, [FromQuery] GetTree query)
    {
        query.Path = Uri.UnescapeDataString(query.Path ?? string.Empty);
        query.RepositoryId = repositoryId;
        return Ok(await _dispatcher.QueryAsync(query));
    }
}