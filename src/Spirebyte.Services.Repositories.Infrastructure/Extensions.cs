using Convey;
using Convey.Auth;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Convey.CQRS.Queries;
using Convey.Discovery.Consul;
using Convey.Docs.Swagger;
using Convey.HTTP;
using Convey.LoadBalancing.Fabio;
using Convey.MessageBrokers.CQRS;
using Convey.MessageBrokers.Outbox;
using Convey.MessageBrokers.Outbox.Mongo;
using Convey.MessageBrokers.RabbitMQ;
using Convey.Metrics.AppMetrics;
using Convey.Metrics.Prometheus;
using Convey.Persistence.MongoDB;
using Convey.Security;
using Convey.Tracing.Jaeger;
using Convey.Tracing.Jaeger.RabbitMQ;
using Convey.WebApi;
using Convey.WebApi.CQRS;
using Convey.WebApi.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Partytitan.Convey.Minio;
using Spirebyte.Services.Repositories.Application;
using Spirebyte.Services.Repositories.Application.Clients.Interfaces;
using Spirebyte.Services.Repositories.Application.Projects.Events.External;
using Spirebyte.Services.Repositories.Application.Repositories.Commands;
using Spirebyte.Services.Repositories.Application.Services.Interfaces;
using Spirebyte.Services.Repositories.Core.Repositories;
using Spirebyte.Services.Repositories.Infrastructure.Clients.HTTP;
using Spirebyte.Services.Repositories.Infrastructure.Decorators;
using Spirebyte.Services.Repositories.Infrastructure.Exceptions;
using Spirebyte.Services.Repositories.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Repositories.Infrastructure.Mongo.Repositories;
using Spirebyte.Services.Repositories.Infrastructure.Services;
using Spirebyte.Shared.Contexts;

namespace Spirebyte.Services.Repositories.Infrastructure;

public static class Extensions
{
    public static IConveyBuilder AddInfrastructure(this IConveyBuilder builder)
    {
        builder.Services.AddTransient<IMessageBroker, MessageBroker>();
        builder.Services.AddSingleton<IRepositoryRepository, RepositoryRepository>();
        builder.Services.AddSingleton<IPullRequestRepository, PullRequestRepository>();
        builder.Services.AddSingleton<IProjectRepository, ProjectRepository>();
        builder.Services.AddTransient<IIdentityApiHttpClient, IdentityApiHttpClient>();
        builder.Services.TryDecorate(typeof(ICommandHandler<>), typeof(OutboxCommandHandlerDecorator<>));
        builder.Services.TryDecorate(typeof(IEventHandler<>), typeof(OutboxEventHandlerDecorator<>));

        builder.Services.AddSharedContexts();

        return builder
            .AddErrorHandler<ExceptionToResponseMapper>()
            .AddQueryHandlers()
            .AddInMemoryQueryDispatcher()
            .AddInMemoryDispatcher()
            .AddJwt()
            .AddHttpClient()
            .AddConsul()
            .AddFabio()
            .AddExceptionToMessageMapper<ExceptionToMessageMapper>()
            .AddRabbitMq(plugins: p => p.AddJaegerRabbitMqPlugin())
            .AddMessageOutbox(o => o.AddMongo())
            .AddMongo()
            .AddJaeger()
            .AddMongoRepository<ProjectDocument, string>("projects")
            .AddMongoRepository<RepositoryDocument, string>("repositories")
            .AddWebApiSwaggerDocs()
            .AddMinio()
            .AddMetrics()
            .AddSecurity();
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
    {
        app.UseErrorHandler()
            .UseSwaggerDocs()
            .UseJaeger()
            .UseConvey()
            .UseAccessTokenValidator()
            .UsePublicContracts<ContractAttribute>()
            .UseAuthentication()
            .UseMetrics()
            .UseRabbitMq()
            .SubscribeCommand<CreateRepository>()
            .SubscribeEvent<ProjectCreated>();

        return app;
    }
}