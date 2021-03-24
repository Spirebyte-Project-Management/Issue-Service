using Convey;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Issues.Application.Services;
using Spirebyte.Services.Issues.Application.Services.Interfaces;

namespace Spirebyte.Services.Issues.Application
{
    public static class Extensions
    {
        public static IConveyBuilder AddApplication(this IConveyBuilder builder)
        {
            builder.Services.AddTransient<IHistoryService, HistoryService>();

            return builder
                .AddCommandHandlers()
                .AddEventHandlers()
                .AddInMemoryCommandDispatcher()
                .AddInMemoryEventDispatcher();
        }
    }
}
