using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Framework.DAL.MongoDb;
using Spirebyte.Services.Repositories.Application.Clients.Interfaces;
using Spirebyte.Services.Repositories.Core.Repositories;
using Spirebyte.Services.Repositories.Infrastructure.Clients.HTTP;
using Spirebyte.Services.Repositories.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Repositories.Infrastructure.Mongo.Repositories;

namespace Spirebyte.Services.Repositories.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IIdentityApiHttpClient, IdentityApiHttpClient>();
        services.AddTransient<IProjectsApiHttpClient, ProjectsApiHttpClient>();

        services.AddMongo(configuration)
            .AddMongoRepository<ProjectDocument, string>("projects")
            .AddMongoRepository<RepositoryDocument, string>("repositories");
        
        services.AddSingleton<IRepositoryRepository, RepositoryRepository>();
        services.AddSingleton<IPullRequestRepository, PullRequestRepository>();
        services.AddSingleton<IProjectRepository, ProjectRepository>();

        return services;
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder)
    {
        return builder;
    }
}