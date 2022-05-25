using System;
using System.Collections.Generic;
using Convey.Auth;
using Spirebyte.Services.Repositories.Tests.Shared.Const;

namespace Spirebyte.Services.Repositories.Tests.Shared.Helpers;

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
    {
        return AuthManager.CreateToken(userId, role, audience, claims).AccessToken;
    }
}