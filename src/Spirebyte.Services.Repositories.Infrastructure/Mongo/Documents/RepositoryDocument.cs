using System;
using System.Collections.Generic;
using Convey.Types;
using Spirebyte.Services.Repositories.Core.Entities;

namespace Spirebyte.Services.Repositories.Infrastructure.Mongo.Documents;

public sealed class RepositoryDocument : IIdentifiable<string>
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ProjectId { get; set; }
    public IEnumerable<Branch> Branches { get; set; }
    public DateTime CreatedAt { get; set; }
}