using Spirebyte.Services.Repositories.Tests.Shared.Const;
using Spirebyte.Services.Repositories.Tests.Shared.Helpers;
using Spirebyte.Services.Repositories.Tests.Shared.Options;

namespace Spirebyte.Services.Repositories.Tests.Shared.Fixtures;

public class PerformanceFixture
{
    public PerformanceFixture()
    {
        PerformanceOptions =
            OptionsHelper.GetOptions<PerformanceOptions>("performance", AppsettingsConst.PerformanceTest);
    }

    public PerformanceOptions PerformanceOptions { get; set; }
}