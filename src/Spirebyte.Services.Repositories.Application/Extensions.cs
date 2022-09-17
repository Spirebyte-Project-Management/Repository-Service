using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Framework.Messaging;
using Spirebyte.Services.Repositories.Application.Background;
using Spirebyte.Services.Repositories.Application.Background.Interfaces;
using Spirebyte.Services.Repositories.Application.Branches.Services;
using Spirebyte.Services.Repositories.Application.Branches.Services.Interfaces;
using Spirebyte.Services.Repositories.Application.Projects.Events.External;
using Spirebyte.Services.Repositories.Application.PullRequests.Services;
using Spirebyte.Services.Repositories.Application.PullRequests.Services.Interfaces;
using Spirebyte.Services.Repositories.Application.Repositories.Commands;
using Spirebyte.Services.Repositories.Application.Repositories.Services;
using Spirebyte.Services.Repositories.Application.Repositories.Services.Interfaces;

namespace Spirebyte.Services.Repositories.Application;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddHostedService<QueuedHostedService>();
        services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();

        services.AddSingleton<IBranchRequestStorage, BranchRequestStorage>();
        services.AddSingleton<IPullRequestRequestStorage, PullRequestRequestStorage>();
        services.AddSingleton<IPullRequestActionRequestStorage, PullRequestActionRequestStorage>();
        services.AddSingleton<IRepositoryRequestStorage, RepositoryRequestStorage>();
        
        services.AddTransient<IRepositoryService, RepositoryService>();
        
        return services;
    }

    public static IApplicationBuilder UseApplication(this IApplicationBuilder app)
    {
        app.Subscribe()
            .Command<CreateRepository>()
            .Event<ProjectCreated>();

        return app;
    }
}