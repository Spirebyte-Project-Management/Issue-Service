using Convey.CQRS.Events;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Issues.API;
using Spirebyte.Services.Issues.Application.Events.External;
using Spirebyte.Services.Issues.Application.Exceptions;
using Spirebyte.Services.Issues.Core.Entities;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents.Mappers;
using Spirebyte.Services.Issues.Tests.Shared.Factories;
using Spirebyte.Services.Issues.Tests.Shared.Fixtures;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Spirebyte.Services.Issues.Tests.Integration.Events
{
    [Collection("Spirebyte collection")]
    public class ProjectCreatedTests : IDisposable
    {
        public ProjectCreatedTests(SpirebyteApplicationFactory<Program> factory)
        {
            _rabbitMqFixture = new RabbitMqFixture();
            _projectsMongoDbFixture = new MongoDbFixture<ProjectDocument, Guid>("projects");
            _usersMongoDbFixture = new MongoDbFixture<UserDocument, Guid>("users");
            factory.Server.AllowSynchronousIO = true;
            _eventHandler = factory.Services.GetRequiredService<IEventHandler<ProjectCreated>>();
        }

        public void Dispose()
        {
            _projectsMongoDbFixture.Dispose();
            _usersMongoDbFixture.Dispose();
        }

        private const string Exchange = "projects";
        private readonly MongoDbFixture<ProjectDocument, Guid> _projectsMongoDbFixture;
        private readonly MongoDbFixture<UserDocument, Guid> _usersMongoDbFixture;
        private readonly RabbitMqFixture _rabbitMqFixture;
        private readonly IEventHandler<ProjectCreated> _eventHandler;


        [Fact]
        public async Task projectcreated_event_should_add_project_with_given_data_to_database()
        {
            var projectId = Guid.NewGuid();
            var key = "key";


            var externalEvent = new ProjectCreated(projectId, key);

            // Check if exception is thrown

            _eventHandler
                .Awaiting(c => c.HandleAsync(externalEvent))
                .Should().NotThrow();


            var project = await _projectsMongoDbFixture.GetAsync(externalEvent.ProjectId);

            project.Should().NotBeNull();
            project.Id.Should().Be(projectId);
            project.Key.Should().Be(key);
        }


        [Fact]
        public async Task projectcreated_event_fails_when_project_with_id_already_exists()
        {
            var projectId = Guid.NewGuid();
            var key = "key";

            var project = new Project(projectId, key);
            await _projectsMongoDbFixture.InsertAsync(project.AsDocument());

            var externalEvent = new ProjectCreated(projectId, key);

            // Check if exception is thrown

            _eventHandler
                .Awaiting(c => c.HandleAsync(externalEvent))
                .Should().Throw<ProjectAlreadyCreatedException>();
        }
    }
}
