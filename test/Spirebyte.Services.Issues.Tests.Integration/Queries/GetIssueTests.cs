using Convey.CQRS.Queries;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Issues.API;
using Spirebyte.Services.Issues.Application.DTO;
using Spirebyte.Services.Issues.Application.Queries;
using Spirebyte.Services.Issues.Core.Entities;
using Spirebyte.Services.Issues.Core.Enums;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents.Mappers;
using Spirebyte.Services.Issues.Tests.Shared.Factories;
using Spirebyte.Services.Issues.Tests.Shared.Fixtures;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Spirebyte.Services.Issues.Tests.Integration.Queries
{
    [Collection("Spirebyte collection")]
    public class GetIssueTests : IDisposable
    {
        public GetIssueTests(SpirebyteApplicationFactory<Program> factory)
        {
            _rabbitMqFixture = new RabbitMqFixture();
            _issuesMongoDbFixture = new MongoDbFixture<IssueDocument, Guid>("issues");
            _projectsMongoDbFixture = new MongoDbFixture<ProjectDocument, Guid>("projects");
            _usersMongoDbFixture = new MongoDbFixture<UserDocument, Guid>("users");
            factory.Server.AllowSynchronousIO = true;
            _queryHandler = factory.Services.GetRequiredService<IQueryHandler<GetIssue, IssueDto>>();
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
        private readonly IQueryHandler<GetIssue, IssueDto> _queryHandler;


        [Fact]
        public async Task getissue_query_succeeds_when_issue_exists()
        {
            var projectId = Guid.NewGuid();
            var issueId = Guid.NewGuid();
            var projectKey = "key";
            var issueKey = "key-1";
            var title = "Title";
            var description = "description";
            var type = IssueType.Story;
            var status = IssueStatus.TODO;
            var storypoints = 0;

            var project = new Project(projectId, projectKey);
            await _projectsMongoDbFixture.InsertAsync(project.AsDocument());

            var issue = new Issue(issueId, issueKey, type, status, title, description, storypoints, projectId, null, null, DateTime.Now);
            await _issuesMongoDbFixture.InsertAsync(issue.AsDocument());

            var query = new GetIssue(issueKey);

            // Check if exception is thrown

            var requestResult = _queryHandler
                .Awaiting(c => c.HandleAsync(query));

            requestResult.Should().NotThrow();

            var result = await requestResult();

            result.Should().NotBeNull();
            result.Id.Should().Be(issueId);
            result.Type.Should().Be(type);
            result.Status.Should().Be(status);
            result.Title.Should().Be(title);
            result.Description.Should().Be(description);
            result.StoryPoints.Should().Be(storypoints);
            result.ProjectId.Should().Be(projectId);
        }

        [Fact]
        public async Task getissue_query_returns_null_when_no_issue_with_key_exists()
        {
            var projectId = Guid.NewGuid();
            var projectKey = "key";
            var issueKey = "key-1";

            var project = new Project(projectId, projectKey);
            await _projectsMongoDbFixture.InsertAsync(project.AsDocument());

            var query = new GetIssue(issueKey);

            // Check if exception is thrown

            var requestResult = _queryHandler
                .Awaiting(c => c.HandleAsync(query));

            requestResult.Should().NotThrow();

            var result = await requestResult();
            result.Should().BeNull();
        }

        [Fact]
        public async Task getissue_query_returns_null_when_project_does_not_exist()
        {
            var projectId = Guid.NewGuid();
            var issueId = Guid.NewGuid();
            var issueKey = "key-1";
            var title = "Title";
            var description = "description";
            var type = IssueType.Story;
            var status = IssueStatus.TODO;
            var storypoints = 0;

            var issue = new Issue(issueId, issueKey, type, status, title, description, storypoints, projectId, null, null, DateTime.Now);
            await _issuesMongoDbFixture.InsertAsync(issue.AsDocument());

            var query = new GetIssue(issueKey);

            // Check if exception is thrown

            var requestResult = _queryHandler
                .Awaiting(c => c.HandleAsync(query));

            requestResult.Should().NotThrow();

            var result = await requestResult();
            result.Should().BeNull();
        }
    }
}
