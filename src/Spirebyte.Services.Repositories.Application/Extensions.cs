using Convey;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Repositories.Application.Repositories.Services;
using Spirebyte.Services.Repositories.Application.Repositories.Services.Interfaces;

namespace Spirebyte.Services.Repositories.Application;

public static class Extensions
{
    public static IConveyBuilder AddApplication(this IConveyBuilder builder)
    {
        builder.Services.AddSingleton<IRepositoryRequestStorage, RepositoryRequestStorage>();

        return builder
            .AddCommandHandlers()
            .AddEventHandlers()
            .AddInMemoryCommandDispatcher()
            .AddInMemoryEventDispatcher();
    }
}