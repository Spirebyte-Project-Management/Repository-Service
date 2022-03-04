using System;
using Spirebyte.Services.Repositories.Application.Repositories.DTO;
using Spirebyte.Services.Repositories.Core.Entities;

namespace Spirebyte.Services.Repositories.Application.Repositories.Services.Interfaces;

public interface IRepositoryRequestStorage
{
    void SetRepository(Guid referenceId, Repository repository);
    RepositoryDto GetRepository(Guid referenceId);
}