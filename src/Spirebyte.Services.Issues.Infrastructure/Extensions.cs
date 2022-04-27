using System;
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
using Convey.Persistence.MongoDB;
using Convey.Persistence.Redis;
using Convey.Security;
using Convey.Tracing.Jaeger;
using Convey.Tracing.Jaeger.RabbitMQ;
using Convey.WebApi;
using Convey.WebApi.CQRS;
using Convey.WebApi.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Issues.Application;
using Spirebyte.Services.Issues.Application.Clients.Interfaces;
using Spirebyte.Services.Issues.Application.Issues.Commands;
using Spirebyte.Services.Issues.Application.Issues.Events.External;
using Spirebyte.Services.Issues.Application.Projects.Events.External;
using Spirebyte.Services.Issues.Application.Services.Interfaces;
using Spirebyte.Services.Issues.Application.Users.Events.External;
using Spirebyte.Services.Issues.Core.Repositories;
using Spirebyte.Services.Issues.Infrastructure.Clients.HTTP;
using Spirebyte.Services.Issues.Infrastructure.Decorators;
using Spirebyte.Services.Issues.Infrastructure.Exceptions;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Repositories;
using Spirebyte.Services.Issues.Infrastructure.Services;
using Spirebyte.Shared.Contexts;

namespace Spirebyte.Services.Issues.Infrastructure;

public static class Extensions
{
    public static IConveyBuilder AddInfrastructure(this IConveyBuilder builder)
    {
        builder.Services.AddTransient<IMessageBroker, MessageBroker>();
        builder.Services.AddTransient<IIssueRepository, IssueRepository>();
        builder.Services.AddTransient<IProjectRepository, ProjectRepository>();
        builder.Services.AddTransient<IUserRepository, UserRepository>();
        builder.Services.AddTransient<ICommentRepository, CommentRepository>();
        builder.Services.AddTransient<IHistoryRepository, HistoryRepository>();
        builder.Services.AddTransient<IProjectsApiHttpClient, ProjectsApiHttpClient>();
        builder.Services.AddTransient<ISprintsApiHttpClient, SprintsApiHttpClient>();

        builder.Services.AddSharedContexts();

        builder.Services.TryDecorate(typeof(ICommandHandler<>), typeof(OutboxCommandHandlerDecorator<>));
        builder.Services.TryDecorate(typeof(IEventHandler<>), typeof(OutboxEventHandlerDecorator<>));

        builder
            .AddErrorHandler<ExceptionToResponseMapper>()
            .AddQueryHandlers()
            .AddInMemoryQueryDispatcher()
            .AddInMemoryDispatcher()
            .AddJwt()
            .AddHttpClient()
            .AddConsul()
            .AddFabio()
            .AddMetrics()
            .AddExceptionToMessageMapper<ExceptionToMessageMapper>()
            .AddRabbitMq(plugins: p => p.AddJaegerRabbitMqPlugin())
            .AddMessageOutbox(o => o.AddMongo())
            .AddMongo()
            .AddRedis()
            .AddJaeger()
            .AddMongoRepository<ProjectDocument, string>("projects")
            .AddMongoRepository<IssueDocument, string>("issues")
            .AddMongoRepository<UserDocument, Guid>("users")
            .AddMongoRepository<CommentDocument, string>("comments")
            .AddMongoRepository<HistoryDocument, Guid>("histories")
            .AddWebApiSwaggerDocs()
            .AddSecurity();

        builder.Services.AddCorrelationContextFactories();
        
        return builder;
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
    {
        app.UseErrorHandler()
            .UseSwaggerDocs()
            .UseJaeger()
            .UseConvey()
            .UseMetrics()
            .UseAccessTokenValidator()
            .UsePublicContracts<ContractAttribute>()
            .UseAuthentication()
            .UseRabbitMq()
            .SubscribeCommand<CreateIssue>()
            .SubscribeEvent<SignedUp>()
            .SubscribeEvent<ProjectCreated>()
            .SubscribeEvent<EndedSprint>()
            .SubscribeEvent<SprintDeleted>()
            .SubscribeEvent<AddedIssueToSprint>()
            .SubscribeEvent<RemovedIssueFromSprint>();

        return app;
    }
}