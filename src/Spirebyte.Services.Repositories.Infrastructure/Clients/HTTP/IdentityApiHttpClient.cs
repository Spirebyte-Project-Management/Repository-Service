﻿using System;
using System.Threading.Tasks;
using Convey.HTTP;
using Spirebyte.Services.Repositories.Application.Clients.DTO;
using Spirebyte.Services.Repositories.Application.Clients.Interfaces;

namespace Spirebyte.Services.Repositories.Infrastructure.Clients.HTTP;

internal sealed class IdentityApiHttpClient : IIdentityApiHttpClient
{
    private readonly IHttpClient _client;
    private readonly string _url;

    public IdentityApiHttpClient(IHttpClient client, HttpClientOptions options)
    {
        _client = client;
        _url = options.Services["identity"];
    }

    public Task<UserDto> GetUserAsync(Guid userId)
    {
        return _client.GetAsync<UserDto>($"{_url}/users/{userId}/");
    }
}