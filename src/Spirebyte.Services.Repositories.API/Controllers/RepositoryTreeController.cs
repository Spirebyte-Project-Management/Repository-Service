using System;
using System.Threading.Tasks;
using Convey.WebApi;
using Convey.WebApi.CQRS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spirebyte.Services.Repositories.API.Controllers.Base;
using Spirebyte.Services.Repositories.Application.Repositories.DTO;
using Spirebyte.Services.Repositories.Application.Repositories.Queries;
using Spirebyte.Services.Repositories.Core.Constants;
using Swashbuckle.AspNetCore.Annotations;

namespace Spirebyte.Services.Repositories.API.Controllers;

[Route("repositories/{repositoryId}/tree")]
public class RepositoryTreeController : BaseController
{
    private readonly IDispatcher _dispatcher;

    public RepositoryTreeController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    [HttpGet]
    [Authorize(ApiScopes.Read)]
    [SwaggerOperation("Browse Repository Tree")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<TreeDto>> BrowseAsync(string repositoryId, [FromQuery] GetTree query)
    {
        query.Path = Uri.UnescapeDataString(query.Path ?? string.Empty);
        return Ok(await _dispatcher.QueryAsync(query.Bind(q => q.RepositoryId, repositoryId)));
    }
}