using System;
using System.Collections.Generic;
using Convey.Types;
using Spirebyte.Services.Repositories.Core.Entities;

namespace Spirebyte.Services.Repositories.Infrastructure.Mongo.Documents;

public sealed class RepositoryDocument : IIdentifiable<string>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string ProjectId { get; set; }
    public Guid ReferenceId { get; set; }
    public List<Branch> Branches { get; set; }
    public List<PullRequest> PullRequests { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Id { get; set; }
}