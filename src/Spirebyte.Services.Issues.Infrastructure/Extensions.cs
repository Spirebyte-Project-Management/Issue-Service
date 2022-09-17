using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Framework.DAL.MongoDb;
using Spirebyte.Services.Issues.Application.Clients.Interfaces;
using Spirebyte.Services.Issues.Core.Repositories;
using Spirebyte.Services.Issues.Infrastructure.Clients.HTTP;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Repositories;

namespace Spirebyte.Services.Issues.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IProjectsApiHttpClient, ProjectsApiHttpClient>();
        services.AddTransient<ISprintsApiHttpClient, SprintsApiHttpClient>();

        services.AddMongo(configuration)
            .AddMongoRepository<ProjectDocument, string>("projects")
            .AddMongoRepository<IssueDocument, string>("issues")
            .AddMongoRepository<UserDocument, Guid>("users")
            .AddMongoRepository<CommentDocument, string>("comments")
            .AddMongoRepository<HistoryDocument, Guid>("histories");
        
        services.AddTransient<IIssueRepository, IssueRepository>();
        services.AddTransient<IProjectRepository, ProjectRepository>();
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<ICommentRepository, CommentRepository>();
        services.AddTransient<IHistoryRepository, HistoryRepository>();

        return services;
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder)
    {
        return builder;
    }
}