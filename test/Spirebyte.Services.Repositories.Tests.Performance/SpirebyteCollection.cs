using Spirebyte.Services.Repositories.Tests.Shared.Fixtures;
using Xunit;

namespace Spirebyte.Services.Repositories.Tests.Performance;

[CollectionDefinition("Spirebyte collection")]
public class SpirebyteCollection : ICollectionFixture<PerformanceFixture>
{
}