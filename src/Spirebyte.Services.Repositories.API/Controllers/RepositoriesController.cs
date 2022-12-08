using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Spirebyte.Framework.API;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Services.Repositories.Application.Repositories.Commands;
using Spirebyte.Services.Repositories.Application.Repositories.DTO;
using Spirebyte.Services.Repositories.Application.Repositories.Queries;
using Spirebyte.Services.Repositories.Application.Repositories.Services.Interfaces;
using Spirebyte.Services.Repositories.Core.Constants;
using Spirebyte.Services.Repositories.Infrastructure.DistributedCache;
using Swashbuckle.AspNetCore.Annotations;

namespace Spirebyte.Services.Repositories.API.Controllers;

[Authorize]
public class RepositoriesController : ApiController
{
    private const string RepositoriesCacheKey = "repositories:";
    private readonly IDistributedCache _cache;
    private readonly IDispatcher _dispatcher;
    private readonly IRepositoryRequestStorage _repositoryRequestStorage;

    public RepositoriesController(IDispatcher dispatcher, IRepositoryRequestStorage repositoryRequestStorage,
        IDistributedCache cache)
    {
        _dispatcher = dispatcher;
        _repositoryRequestStorage = repositoryRequestStorage;
        _cache = cache;
    }

    [HttpGet]
    [Authorize(ApiScopes.RepositoriesRead)]
    [SwaggerOperation("Browse Repositories")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<RepositoryDto>>> BrowseAsync([FromQuery] GetRepositories query)
    {
        var cacheKey = RepositoriesCacheKey + query.ProjectId;
        if (_cache.TryGetValue(cacheKey, out List<RepositoryDto> repositoryDtos)) return Ok(repositoryDtos);

        var repositories = await _dispatcher.QueryAsync(query);

        var cacheEntryOptions = new DistributedCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromSeconds(60))
            .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600));
        await _cache.SetAsync(cacheKey, repositories, cacheEntryOptions);

        return Ok(repositories);
    }

    [HttpGet("{repositoryId}")]
    [Authorize(ApiScopes.RepositoriesRead)]
    [SwaggerOperation("Get Repository")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RepositoryDto?>> GetAsync(string repositoryId)
    {
        return await _dispatcher.QueryAsync(new GetRepository(repositoryId));
    }

    [HttpPost]
    [Authorize(ApiScopes.RepositoriesWrite)]
    [SwaggerOperation("Create Repository")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateRepository(CreateRepository command)
    {
        await _dispatcher.SendAsync(command);

        var cacheKey = RepositoriesCacheKey + command.ProjectId;
        await _cache.RemoveAsync(cacheKey);

        var repository = _repositoryRequestStorage.GetRepository(command.ReferenceId);
        return Created($"Repositories/{repository.Id}", repository);
    }


    [HttpPut("{repositoryId}")]
    [Authorize(ApiScopes.RepositoriesWrite)]
    [SwaggerOperation("Update Repository")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdatePermissionScheme(string repositoryId, UpdateRepository command)
    {
        if (!command.Id.Equals(repositoryId)) return NotFound();

        await _dispatcher.SendAsync(command);

        var cacheKey = RepositoriesCacheKey + command.ProjectId;
        await _cache.RemoveAsync(cacheKey);

        return Ok();
    }

    [HttpDelete("{repositoryId}")]
    [Authorize(ApiScopes.RepositoriesDelete)]
    [SwaggerOperation("Delete Repository")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteRepository(string repositoryId)
    {
        await _dispatcher.SendAsync(new DeleteRepository(repositoryId));

        return Ok();
    }
}