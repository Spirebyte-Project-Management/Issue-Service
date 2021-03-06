using Convey.CQRS.Commands;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Issues.API;
using Spirebyte.Services.Issues.Application.Commands;
using Spirebyte.Services.Issues.Application.Exceptions;
using Spirebyte.Services.Issues.Core.Entities;
using Spirebyte.Services.Issues.Core.Enums;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents.Mappers;
using Spirebyte.Services.Issues.Tests.Shared.Factories;
using Spirebyte.Services.Issues.Tests.Shared.Fixtures;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Spirebyte.Services.Issues.Tests.Integration.Commands
{
    [Collection("Spirebyte collection")]
    public class DeleteIssueTests : IDisposable
    {
        public DeleteIssueTests(SpirebyteApplicationFactory<Program> factory)
        {
            _rabbitMqFixture = new RabbitMqFixture();
            _issuesMongoDbFixture = new MongoDbFixture<IssueDocument, string>("issues");
            _projectsMongoDbFixture = new MongoDbFixture<ProjectDocument, string>("projects");
            _usersMongoDbFixture = new MongoDbFixture<UserDocument, Guid>("users");
            factory.Server.AllowSynchronousIO = true;
            _commandHandler = factory.Services.GetRequiredService<ICommandHandler<DeleteIssue>>();
        }

        public void Dispose()
        {
            _issuesMongoDbFixture.Dispose();
            _projectsMongoDbFixture.Dispose();
            _usersMongoDbFixture.Dispose();
        }

        private const string Exchange = "issues";
        private readonly MongoDbFixture<IssueDocument, string> _issuesMongoDbFixture;
        private readonly MongoDbFixture<ProjectDocument, string> _projectsMongoDbFixture;
        private readonly MongoDbFixture<UserDocument, Guid> _usersMongoDbFixture;
        private readonly RabbitMqFixture _rabbitMqFixture;
        private readonly ICommandHandler<DeleteIssue> _commandHandler;


        [Fact]
        public async Task delete_issue_command_should_remove_issue_with_given_key()
        {
            var projectId = "projectKey";
            var epicId = "epicKey";
            var issueId = "issueKey";
            var sprintId = string.Empty;
            var title = "Title";
            var description = "description";
            var type = IssueType.Story;
            var status = IssueStatus.TODO;
            var storypoints = 0;

            var issue = new Issue(issueId, type, status, title, description, storypoints, projectId, epicId, sprintId, null, null, DateTime.Now);
            await _issuesMongoDbFixture.InsertAsync(issue.AsDocument());

            var command = new DeleteIssue(issueId);

            // Check if exception is thrown

            _commandHandler
                .Awaiting(c => c.HandleAsync(command))
                .Should().NotThrow();


            var updatedIssue = await _issuesMongoDbFixture.GetAsync(issueId);

            updatedIssue.Should().BeNull();
        }

        [Fact]
        public async Task delete_issue_command_fails_when_issue_with_key_does_not_exist()
        {
            var issueId = "key-1";

            var command = new DeleteIssue(issueId);

            // Check if exception is thrown

            _commandHandler
                .Awaiting(c => c.HandleAsync(command))
                .Should().Throw<IssueNotFoundException>();
        }
    }
}
