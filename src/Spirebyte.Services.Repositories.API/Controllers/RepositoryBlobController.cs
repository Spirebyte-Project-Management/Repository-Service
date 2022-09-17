using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Services.Repositories.API.Controllers.Base;
using Spirebyte.Services.Repositories.Application.Repositories.DTO;
using Spirebyte.Services.Repositories.Application.Repositories.Queries;
using Spirebyte.Services.Repositories.Core.Constants;
using Swashbuckle.AspNetCore.Annotations;

namespace Spirebyte.Services.Repositories.API.Controllers;

[Route("repositories/{repositoryId}/blob")]
public class RepositoryBlobController : BaseController
{
    private readonly IDispatcher _dispatcher;

    public RepositoryBlobController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    [HttpGet]
    [Authorize(ApiScopes.Read)]
    [SwaggerOperation("Get Repository Blob")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<BlobDto>> GetAsync(string repositoryId, [FromQuery] GetBlob query)
    {
        if (string.IsNullOrEmpty(query.Path)) return BadRequest();

        query.Path = Uri.UnescapeDataString(query.Path ?? string.Empty);
        query.RepositoryId = repositoryId;
        return Ok(await _dispatcher.QueryAsync(query));
    }
}