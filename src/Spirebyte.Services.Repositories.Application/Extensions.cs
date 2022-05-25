using Convey;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Repositories.Application.Background;
using Spirebyte.Services.Repositories.Application.Background.Interfaces;
using Spirebyte.Services.Repositories.Application.Branches.Services;
using Spirebyte.Services.Repositories.Application.Branches.Services.Interfaces;
using Spirebyte.Services.Repositories.Application.PullRequests.Services;
using Spirebyte.Services.Repositories.Application.PullRequests.Services.Interfaces;
using Spirebyte.Services.Repositories.Application.Repositories.Services;
using Spirebyte.Services.Repositories.Application.Repositories.Services.Interfaces;

namespace Spirebyte.Services.Repositories.Application;

public static class Extensions
{
    public static IConveyBuilder AddApplication(this IConveyBuilder builder)
    {
        builder.Services.AddHostedService<QueuedHostedService>();
        builder.Services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();

        builder.Services.AddSingleton<IBranchRequestStorage, BranchRequestStorage>();
        builder.Services.AddSingleton<IPullRequestRequestStorage, PullRequestRequestStorage>();
        builder.Services.AddSingleton<IPullRequestActionRequestStorage, PullRequestActionRequestStorage>();
        builder.Services.AddSingleton<IRepositoryRequestStorage, RepositoryRequestStorage>();
        builder.Services.AddSingleton<IRepositoryService, RepositoryService>();

        return builder
            .AddCommandHandlers()
            .AddEventHandlers()
            .AddInMemoryCommandDispatcher()
            .AddInMemoryEventDispatcher();
    }
}