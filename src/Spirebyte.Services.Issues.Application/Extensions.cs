using Convey;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Issues.Application.IssueComments.Services;
using Spirebyte.Services.Issues.Application.IssueComments.Services.Interfaces;
using Spirebyte.Services.Issues.Application.Issues.Services;
using Spirebyte.Services.Issues.Application.Issues.Services.Interfaces;

namespace Spirebyte.Services.Issues.Application;

public static class Extensions
{
    public static IConveyBuilder AddApplication(this IConveyBuilder builder)
    {
        builder.Services.AddTransient<IHistoryService, HistoryService>();
        builder.Services.AddSingleton<IIssueRequestStorage, IssueRequestStorage>();
        builder.Services.AddSingleton<IIssueCommentsRequestStorage, IssueCommentsRequestStorage>();

        return builder
            .AddCommandHandlers()
            .AddEventHandlers()
            .AddInMemoryCommandDispatcher()
            .AddInMemoryEventDispatcher();
    }
}