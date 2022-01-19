using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
using Xunit;

namespace Spirebyte.Services.Issues.Tests.Integration.Queries;

[Collection("Spirebyte collection")]
public class GetIssuesTests : IDisposable
{
    private const string Exchange = "issues";
    private readonly MongoDbFixture<IssueDocument, string> _issuesMongoDbFixture;
    private readonly MongoDbFixture<ProjectDocument, string> _projectsMongoDbFixture;
    private readonly IQueryHandler<GetIssues, IEnumerable<IssueDto>> _queryHandler;
    private readonly RabbitMqFixture _rabbitMqFixture;
    private readonly MongoDbFixture<UserDocument, Guid> _usersMongoDbFixture;

    public GetIssuesTests(SpirebyteApplicationFactory<Program> factory)
    {
        _rabbitMqFixture = new RabbitMqFixture();
        _issuesMongoDbFixture = new MongoDbFixture<IssueDocument, string>("issues");
        _projectsMongoDbFixture = new MongoDbFixture<ProjectDocument, string>("projects");
        _usersMongoDbFixture = new MongoDbFixture<UserDocument, Guid>("users");
        factory.Server.AllowSynchronousIO = true;
        _queryHandler = factory.Services.GetRequiredService<IQueryHandler<GetIssues, IEnumerable<IssueDto>>>();
    }

    public void Dispose()
    {
        _issuesMongoDbFixture.Dispose();
        _projectsMongoDbFixture.Dispose();
        _usersMongoDbFixture.Dispose();
    }


    [Fact]
    public async Task getissues_query_succeeds_when_a_issue_exists()
    {
        var projectId = "projectKey";
        var epicId = "epicKey";
        var issueId = "issueKey";
        var issue2Id = "issueKey2";
        var sprintId = string.Empty;
        var title = "Title";
        var description = "description";
        var type = IssueType.Story;
        var status = IssueStatus.TODO;
        var storypoints = 0;

        var project = new Project(projectId);
        await _projectsMongoDbFixture.InsertAsync(project.AsDocument());

        var issue = new Issue(issueId, type, status, title, description, storypoints, projectId, epicId, sprintId, null,
            null, DateTime.Now);
        var issue2 = new Issue(issue2Id, type, status, title, description, storypoints, projectId, epicId, sprintId,
            null, null, DateTime.Now);
        await _issuesMongoDbFixture.InsertAsync(issue.AsDocument());
        await _issuesMongoDbFixture.InsertAsync(issue2.AsDocument());

        var query = new GetIssues(projectId);

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
        var projectId = "projectKey";

        var project = new Project(projectId);
        await _projectsMongoDbFixture.InsertAsync(project.AsDocument());

        var query = new GetIssues(projectId);

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
        var projectId = "projectKey";
        var epicId = "epicKey";
        var issueId = "issueKey";
        var sprintId = string.Empty;
        var title = "Title";
        var description = "description";
        var type = IssueType.Story;
        var status = IssueStatus.TODO;
        var storypoints = 0;

        var issue = new Issue(issueId, type, status, title, description, storypoints, projectId, epicId, sprintId, null,
            null, DateTime.Now);
        await _issuesMongoDbFixture.InsertAsync(issue.AsDocument());

        var query = new GetIssues(projectId);

        // Check if exception is thrown

        var requestResult = _queryHandler
            .Awaiting(c => c.HandleAsync(query));

        requestResult.Should().NotThrow();

        var result = await requestResult();
        result.Should().BeEmpty();
    }
}