using System.Threading.Tasks;
using Convey;
using Convey.Logging;
using Convey.Secrets.Vault;
using Convey.Types;
using Convey.WebApi;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Repositories.Application;
using Spirebyte.Services.Repositories.Core.Constants;
using Spirebyte.Services.Repositories.Infrastructure;
using Spirebyte.Shared.IdentityServer;

namespace Spirebyte.Services.Repositories.API;

public class Program
{
    public static async Task Main(string[] args)
    {
        await CreateWebHostBuilder(args)
            .Build()
            .RunAsync();
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args)
    {
        return WebHost.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddControllers().AddMetrics();
                services.AddAuthorization(options =>
                {
                    options.AddEitherOrScopePolicy(ApiScopes.Read, "repositories.read", "repositories.manage");
                    options.AddEitherOrScopePolicy(ApiScopes.Write, "repositories.write", "repositories.manage");
                    options.AddEitherOrScopePolicy(ApiScopes.Delete, "repositories.delete", "repositories.manage");
                    options.AddEitherOrScopePolicy(ApiScopes.Commit, "repositories.commit", "repositories.manage");
                });
                services
                    .AddConvey()
                    .AddWebApi()
                    .AddApplication()
                    .AddInfrastructure()
                    .Build();
            })
            .Configure(app => app
                .UseInfrastructure()
                .UseRouting()
                .UseAuthorization()
                .UsePingEndpoint()
                .UseEndpoints(endpoints =>
                    {
                        endpoints.MapGet("",
                            ctx => ctx.Response.WriteAsync(ctx.RequestServices.GetService<AppOptions>()?.Name!));
                        endpoints.MapControllers();
                    }
                ))
            .UseLogging()
            .UseVault();
    }
}