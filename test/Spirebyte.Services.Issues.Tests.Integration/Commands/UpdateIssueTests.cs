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
    public class UpdateIssueTests : IDisposable
    {
        public UpdateIssueTests(SpirebyteApplicationFactory<Program> factory)
        {
            _rabbitMqFixture = new RabbitMqFixture();
            _issuesMongoDbFixture = new MongoDbFixture<IssueDocument, Guid>("issues");
            _projectsMongoDbFixture = new MongoDbFixture<ProjectDocument, Guid>("projects");
            _usersMongoDbFixture = new MongoDbFixture<UserDocument, Guid>("users");
            factory.Server.AllowSynchronousIO = true;
            _commandHandler = factory.Services.GetRequiredService<ICommandHandler<UpdateIssue>>();
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
        private readonly ICommandHandler<UpdateIssue> _commandHandler;


        [Fact]
        public async Task update_issue_command_should_update_issue_with_given_data_to()
        {
            var projectId = Guid.NewGuid();
            var issueId = Guid.NewGuid();
            var issueKey = "key-1";
            var title = "Title";
            var updatedTitle = "UpdatedTitle";
            var description = "description";
            var updatedDescription = "updatedDescription";
            var type = IssueType.Story;
            var status = IssueStatus.TODO;
            var storypoints = 0;

            var issue = new Issue(issueId, issueKey, type, status, title, description, storypoints, projectId, null, null, DateTime.Now);
            await _issuesMongoDbFixture.InsertAsync(issue.AsDocument());

            var command = new UpdateIssue(issueId, issueKey, type, status, updatedTitle, updatedDescription, storypoints, null, null);

            // Check if exception is thrown

            _commandHandler
                .Awaiting(c => c.HandleAsync(command))
                .Should().NotThrow();


            var updatedIssue = await _issuesMongoDbFixture.GetAsync(command.IssueId);

            updatedIssue.Should().NotBeNull();
            updatedIssue.Id.Should().Be(issueId);
            updatedIssue.Title.Should().Be(updatedTitle);
            updatedIssue.Description.Should().Be(updatedDescription);
        }

        [Fact]
        public async Task update_issue_command_fails_when_issue_with_key_does_not_exist()
        {
            var projectId = Guid.NewGuid();
            var issueId = Guid.NewGuid();
            var issueKey = "key-1";
            var title = "Title";
            var description = "description";
            var type = IssueType.Story;
            var status = IssueStatus.TODO;
            var storypoints = 0;

            var command = new UpdateIssue(issueId, issueKey, type, status, title, description, storypoints, null, null);

            // Check if exception is thrown

            _commandHandler
                .Awaiting(c => c.HandleAsync(command))
                .Should().Throw<IssueNotFoundException>();
        }
    }
}
