using Spirebyte.Services.Repositories.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Repositories.Tests.Shared.Infrastructure.Mongo;

namespace Spirebyte.Services.Repositories.Tests.Acceptance.Drivers;

public class MongoDbDriver
{
    public MongoDbTestRepository<ProjectDocument, string> ProjectRepository;
    public MongoDbTestRepository<RepositoryDocument, string> RepositoryRepository;

    public MongoDbDriver()
    {
        ProjectRepository =
            new MongoDbTestRepository<ProjectDocument, string>("projects", SettingsConst.AcceptanceTestsSettings);
        RepositoryRepository =
            new MongoDbTestRepository<RepositoryDocument, string>("repositories",
                SettingsConst.AcceptanceTestsSettings);
    }
}