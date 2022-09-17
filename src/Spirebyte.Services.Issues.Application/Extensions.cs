using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Framework.Messaging;
using Spirebyte.Services.Issues.Application.IssueComments.Services;
using Spirebyte.Services.Issues.Application.IssueComments.Services.Interfaces;
using Spirebyte.Services.Issues.Application.Issues.Events.External;
using Spirebyte.Services.Issues.Application.Issues.Services;
using Spirebyte.Services.Issues.Application.Issues.Services.Interfaces;
using Spirebyte.Services.Issues.Application.Projects.Events.External;
using Spirebyte.Services.Issues.Application.Users.Events.External;

namespace Spirebyte.Services.Issues.Application;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddTransient<IHistoryService, HistoryService>();
        services.AddSingleton<IIssueRequestStorage, IssueRequestStorage>();
        services.AddSingleton<IIssueCommentsRequestStorage, IssueCommentsRequestStorage>();

        return services;
    }

    public static IApplicationBuilder UseApplication(this IApplicationBuilder app)
    {
        app.Subscribe()
            .Event<SignedUp>()
            .Event<ProjectCreated>()
            .Event<EndedSprint>()
            .Event<SprintDeleted>()
            .Event<AddedIssueToSprint>()
            .Event<RemovedIssueFromSprint>();

        return app;
    }
}