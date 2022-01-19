﻿using System;
using Convey.MessageBrokers.RabbitMQ;
using Spirebyte.Services.Issues.Application.Events.Rejected;
using Spirebyte.Services.Issues.Application.Exceptions;

namespace Spirebyte.Services.Issues.Infrastructure.Exceptions;

internal sealed class ExceptionToMessageMapper : IExceptionToMessageMapper
{
    public object Map(Exception exception, object message)
    {
        return exception switch

        {
            ProjectNotFoundException ex => new IssueCreatedRejected(ex.ProjectId, ex.Message, ex.Code),
            _ => null
        };
    }
}