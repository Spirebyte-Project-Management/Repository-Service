using Convey.Auth;
using Spirebyte.Services.Repositories.Tests.Shared.Const;
using Spirebyte.Services.Repositories.Tests.Shared.Helpers;

namespace Spirebyte.Services.Repositories.Tests.Acceptance.Authentication;

public static class AuthHelper
{
    private static readonly AuthManager AuthManager;

    static AuthHelper()
    {
        var options = OptionsHelper.GetOptions<JwtOptions>("jwt", AppsettingsConst.AcceptanceTest);
        AuthManager = new AuthManager(options);
    }

    public static string GenerateJwt(Guid userId, string role = null, string audience = null,
        IDictionary<string, IEnumerable<string>> claims = null)
        => AuthManager.CreateToken(userId, role, audience, claims).AccessToken;
}