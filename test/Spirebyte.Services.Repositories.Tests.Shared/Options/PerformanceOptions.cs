using System;

namespace Spirebyte.Services.Repositories.Tests.Shared.Options;

public class PerformanceOptions
{
    public string BaseUrl { get; set; }
    public Guid UserId { get; set; }
    public string ProjectId { get; set; }
    public string RepositoryId { get; set; }
}