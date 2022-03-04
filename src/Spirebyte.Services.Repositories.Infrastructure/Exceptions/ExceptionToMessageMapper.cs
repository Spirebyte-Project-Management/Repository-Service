using System;
using Convey.MessageBrokers.RabbitMQ;
using Spirebyte.Services.Repositories.Application.Projects.Exceptions;
using Spirebyte.Services.Repositories.Application.Repositories.Events.Rejected;
using Spirebyte.Services.Repositories.Application.Repositories.Exceptions;

namespace Spirebyte.Services.Repositories.Infrastructure.Exceptions;

internal sealed class ExceptionToMessageMapper : IExceptionToMessageMapper
{
    public object Map(Exception exception, object message)
    {
        return exception switch

        {
            ProjectNotFoundException ex => new RepositoryCreatedRejected(ex.ProjectId, ex.Message, ex.Code),
            KeyAlreadyExistsException ex => new RepositoryCreatedRejected(ex.RepositoryId, ex.Message, ex.Code),
            _ => null
        };
    }
}