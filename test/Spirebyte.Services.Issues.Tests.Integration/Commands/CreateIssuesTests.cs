using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Issues.API;
using Spirebyte.Services.Issues.Application.Commands;
using Spirebyte.Services.Issues.Application.Exceptions;
using Spirebyte.Services.Issues.Core.Entities;
using Spirebyte.Services.Issues.Core.Entities.Base;
using Spirebyte.Services.Issues.Core.Enums;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents.Mappers;
using Spirebyte.Services.Issues.Tests.Shared.Factories;
using Spirebyte.Services.Issues.Tests.Shared.Fixtures;
using Xunit;

namespace Spirebyte.Services.Issues.Tests.Integration.Commands
{
    [Collection("Spirebyte collection")]
    public class CreateIssuesTests : IDisposable
    {
        public CreateIssuesTests(SpirebyteApplicationFactory<Program> factory)
        {
            _rabbitMqFixture = new RabbitMqFixture();
            _issuesMongoDbFixture = new MongoDbFixture<IssueDocument, Guid>("issues");
            _projectsMongoDbFixture = new MongoDbFixture<ProjectDocument, Guid>("projects");
            _usersMongoDbFixture = new MongoDbFixture<UserDocument, Guid>("users");
            factory.Server.AllowSynchronousIO = true;
            _commandHandler = factory.Services.GetRequiredService<ICommandHandler<CreateIssue>>();
        }

        public void Dispose()
        {
            _projectsMongoDbFixture.Dispose();
            _usersMongoDbFixture.Dispose();
        }

        private const string Exchange = "issues";
        private readonly MongoDbFixture<IssueDocument, Guid> _issuesMongoDbFixture;
        private readonly MongoDbFixture<ProjectDocument, Guid> _projectsMongoDbFixture;
        private readonly MongoDbFixture<UserDocument, Guid> _usersMongoDbFixture;
        private readonly RabbitMqFixture _rabbitMqFixture;
        private readonly ICommandHandler<CreateIssue> _commandHandler;


        [Fact]
        public async Task create_issue_command_should_add_issue_with_given_data_to_database()
        {
            var projectId = Guid.NewGuid();
            var issueId = Guid.NewGuid();
            var projectKey = "key";
            var title = "Title";
            var description = "description";
            var type = IssueType.Story;
            var status = IssueStatus.TODO;
            var storypoints = 0;

            var project = new Project(projectId, projectKey);
            await _projectsMongoDbFixture.InsertAsync(project.AsDocument());


            var command = new CreateIssue(issueId, projectKey, type, status, title, description, storypoints, projectId, null, null, DateTime.Now);

            // Check if exception is thrown

            _commandHandler
                .Awaiting(c => c.HandleAsync(command))
                .Should().NotThrow();


            var issue = await _issuesMongoDbFixture.GetAsync(command.IssueId);

            issue.Should().NotBeNull();
            issue.Id.Should().Be(issueId);
            issue.Type.Should().Be(type);
            issue.Status.Should().Be(status);
            issue.Title.Should().Be(title);
            issue.Description.Should().Be(description);
            issue.StoryPoints.Should().Be(storypoints);
            issue.ProjectId.Should().Be(projectId);
        }

        [Fact]
        public async Task create_issue_command_fails_when_project_does_not_exist()
        {
            var projectId = Guid.NewGuid();
            var issueId = Guid.NewGuid();
            var projectKey = "key";
            var title = "Title";
            var description = "description";
            var type = IssueType.Story;
            var status = IssueStatus.TODO;
            var storypoints = 0;

            var command = new CreateIssue(issueId, projectKey, type, status, title, description, storypoints, projectId, null, null, DateTime.Now);

            // Check if exception is thrown

            _commandHandler
                .Awaiting(c => c.HandleAsync(command))
                .Should().Throw<ProjectNotFoundException>();
        }
    }
}
