using System.Net;
using RestSharp;
using RestSharp.Authenticators;
using Spirebyte.Framework.Tests.Shared.Helpers;
using Spirebyte.Framework.Tests.Shared.Options;
using Spirebyte.Services.Repositories.Application.Repositories.Commands;

namespace Spirebyte.Services.Repositories.Tests.Acceptance.API;

public class RepositoryApi
{
    private readonly RestClient _client;

    public RepositoryApi()
    {
        var testEnvOptions = OptionsHelper.GetOptions<TestEnvOptions>("Spirebyte.Services.Repositories.API",
            SettingsConst.AcceptanceTestsSettings);


        var options = new RestClientOptions(testEnvOptions.TestingAddress)
        {
            ThrowOnAnyError = false
        };

        _client = new RestClient(options);

        ServicePointManager.ServerCertificateValidationCallback +=
            (sender, cert, chain, sslPolicyErrors) => true;
    }

    public async Task<RestResponse> CreateAsync(CreateRepository command, Guid userId)
    {
        var request = new RestRequest("repositories").AddJsonBody(command);

        _client.UseAuthenticator(new JwtAuthenticator(AuthHelper.GenerateJwt(userId)));

        return await _client.ExecutePostAsync(request);
    }

    public async Task<RestResponse> GetByIdAsync(string id, Guid userId)
    {
        var request = new RestRequest($"repositories/{id}");

        _client.UseAuthenticator(new JwtAuthenticator(AuthHelper.GenerateJwt(userId)));

        return await _client.ExecuteGetAsync(request);
    }
}