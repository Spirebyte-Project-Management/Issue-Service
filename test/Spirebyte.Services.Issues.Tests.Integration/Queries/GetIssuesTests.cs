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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Spirebyte.Services.Issues.Tests.Integration.Queries
{
    [Collection("Spirebyte collection")]
    public class GetIssuesTests : IDisposable
    {
        public GetIssuesTests(SpirebyteApplicationFactory<Program> factory)
        {
            _rabbitMqFixture = new RabbitMqFixture();
            _issuesMongoDbFixture = new MongoDbFixture<IssueDocument, Guid>("issues");
            _projectsMongoDbFixture = new MongoDbFixture<ProjectDocument, Guid>("projects");
            _usersMongoDbFixture = new MongoDbFixture<UserDocument, Guid>("users");
            factory.Server.AllowSynchronousIO = true;
            _queryHandler = factory.Services.GetRequiredService<IQueryHandler<GetIssues, IEnumerable<IssueDto>>>();
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
        private readonly IQueryHandler<GetIssues, IEnumerable<IssueDto>> _queryHandler;


        [Fact]
        public async Task getissues_query_succeeds_when_a_issue_exists()
        {
            var projectId = Guid.NewGuid();
            var issueId = Guid.NewGuid();
            var issue2Id = Guid.NewGuid();
            var projectKey = "key";
            var issueKey = "key-1";
            var issue2Key = "key-2";
            var title = "Title";
            var description = "description";
            var type = IssueType.Story;
            var status = IssueStatus.TODO;
            var storypoints = 0;

            var project = new Project(projectId, projectKey);
            await _projectsMongoDbFixture.InsertAsync(project.AsDocument());

            var issue = new Issue(issueId, issueKey, type, status, title, description, storypoints, projectId, null, null, DateTime.Now);
            var issue2 = new Issue(issue2Id, issue2Key, type, status, title, description, storypoints, projectId, null, null, DateTime.Now);
            await _issuesMongoDbFixture.InsertAsync(issue.AsDocument());
            await _issuesMongoDbFixture.InsertAsync(issue2.AsDocument());

            var query = new GetIssues(projectKey);

            // Check if exception is thrown

            var requestResult = _queryHandler
                .Awaiting(c => c.HandleAsync(query));

            requestResult.Should().NotThrow();

            var result = await requestResult();

            var issueDtos = result as IssueDto[] ?? result.ToArray();
            issueDtos.Should().Contain(i => i.Id == issueId);
            issueDtos.Should().Contain(i => i.Id == issue2Id);
        }

        [Fact]
        public async Task getissues_query_returns_empty_when_project_has_no_issues()
        {
            var projectId = Guid.NewGuid();
            var projectKey = "key";

            var project = new Project(projectId, projectKey);
            await _projectsMongoDbFixture.InsertAsync(project.AsDocument());

            var query = new GetIssues(projectKey);

            // Check if exception is thrown

            var requestResult = _queryHandler
                .Awaiting(c => c.HandleAsync(query));

            requestResult.Should().NotThrow();

            var result = await requestResult();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task getissues_query_returns_empty_when_project_does_not_exist()
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

            var issue = new Issue(issueId, issueKey, type, status, title, description, storypoints, projectId, null, null, DateTime.Now);
            await _issuesMongoDbFixture.InsertAsync(issue.AsDocument());

            var query = new GetIssues(projectKey);

            // Check if exception is thrown

            var requestResult = _queryHandler
                .Awaiting(c => c.HandleAsync(query));

            requestResult.Should().NotThrow();

            var result = await requestResult();
            result.Should().BeEmpty();
        }
    }
}
