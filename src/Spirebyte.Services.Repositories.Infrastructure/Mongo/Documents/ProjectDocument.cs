using Spirebyte.Framework.Shared.Types;

namespace Spirebyte.Services.Repositories.Infrastructure.Mongo.Documents;

public sealed class ProjectDocument : IIdentifiable<string>
{
    public string Id { get; set; }
}