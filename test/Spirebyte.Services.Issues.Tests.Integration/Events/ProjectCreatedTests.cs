using System;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Issues.API;
using Spirebyte.Services.Issues.Application.Exceptions;
using Spirebyte.Services.Issues.Application.Projects.Events.External;
using Spirebyte.Services.Issues.Core.Entities;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents.Mappers;
using Spirebyte.Services.Issues.Tests.Shared.Factories;
using Spirebyte.Services.Issues.Tests.Shared.Fixtures;
using Xunit;

namespace Spirebyte.Services.Issues.Tests.Integration.Events;

[Collection("Spirebyte collection")]
public class ProjectCreatedTests : IDisposable
{
    private const string Exchange = "projects";
    private readonly IEventHandler<ProjectCreated> _eventHandler;
    private readonly MongoDbFixture<ProjectDocument, string> _projectsMongoDbFixture;
    private readonly RabbitMqFixture _rabbitMqFixture;
    private readonly MongoDbFixture<UserDocument, Guid> _usersMongoDbFixture;

    public ProjectCreatedTests(SpirebyteApplicationFactory<Program> factory)
    {
        _rabbitMqFixture = new RabbitMqFixture();
        _projectsMongoDbFixture = new MongoDbFixture<ProjectDocument, string>("projects");
        _usersMongoDbFixture = new MongoDbFixture<UserDocument, Guid>("users");
        factory.Server.AllowSynchronousIO = true;
        _eventHandler = factory.Services.GetRequiredService<IEventHandler<ProjectCreated>>();
    }

    public void Dispose()
    {
        _projectsMongoDbFixture.Dispose();
        _usersMongoDbFixture.Dispose();
    }


    [Fact]
    public async Task projectcreated_event_should_add_project_with_given_data_to_database()
    {
        var projectId = "projectKey";

        var externalEvent = new ProjectCreated { Id = projectId };

        // Check if exception is thrown

        await _eventHandler
            .Awaiting(c => c.HandleAsync(externalEvent))
            .Should().NotThrowAsync();


        var project = await _projectsMongoDbFixture.GetAsync(externalEvent.Id);

        project.Should().NotBeNull();
        project.Id.Should().Be(projectId);
    }


    [Fact]
    public async Task projectcreated_event_fails_when_project_with_id_already_exists()
    {
        var projectId = "projectKey";

        var project = new Project(projectId);
        await _projectsMongoDbFixture.InsertAsync(project.AsDocument());

        var externalEvent = new ProjectCreated { Id = projectId };

        // Check if exception is thrown

        await _eventHandler
            .Awaiting(c => c.HandleAsync(externalEvent))
            .Should().ThrowAsync<ProjectAlreadyCreatedException>();
    }
}