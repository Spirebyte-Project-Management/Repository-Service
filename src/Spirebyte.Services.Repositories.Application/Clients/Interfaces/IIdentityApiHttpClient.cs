using System;
using System.Threading.Tasks;
using Spirebyte.Services.Repositories.Application.Clients.DTO;

namespace Spirebyte.Services.Repositories.Application.Clients.Interfaces;

public interface IIdentityApiHttpClient
{
    Task<UserDto> GetUserAsync(Guid userId);
}