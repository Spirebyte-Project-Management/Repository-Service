using System.Linq;
using Spirebyte.Services.Repositories.Application.Repositories.DTO;
using Spirebyte.Services.Repositories.Core.Entities;

namespace Spirebyte.Services.Repositories.Infrastructure.Mongo.Documents.Mappers;

internal static class RepositoryMappers
{
    public static Repository AsEntity(this RepositoryDocument document)
    {
        return new Repository(document.Id, document.Title, document.Description, document.ProjectId, document.ReferenceId, document.Branches, document.CreatedAt);
    }

    public static RepositoryDocument AsDocument(this Repository entity)
    {
        return new RepositoryDocument
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            ProjectId = entity.ProjectId,
            Branches = entity.Branches,
            ReferenceId = entity.ReferenceId,
            CreatedAt = entity.CreatedAt
        };
    }

    public static RepositoryDto AsDto(this RepositoryDocument document)
    {
        return new RepositoryDto
        {
            Id = document.Id,
            Title = document.Title,
            Description = document.Description,
            ProjectId = document.ProjectId,
            Branches = document.Branches,
            CreatedAt = document.CreatedAt
        };
    }
}