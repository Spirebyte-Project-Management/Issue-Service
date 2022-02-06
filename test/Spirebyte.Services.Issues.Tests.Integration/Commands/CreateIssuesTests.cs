using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Issues.API;
using Spirebyte.Services.Issues.Application.Exceptions;
using Spirebyte.Services.Issues.Application.Issues.Commands;
using Spirebyte.Services.Issues.Application.Issues.Exceptions;
using Spirebyte.Services.Issues.Core.Entities;
using Spirebyte.Services.Issues.Core.Enums;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents.Mappers;
using Spirebyte.Services.Issues.Tests.Shared.Factories;
using Spirebyte.Services.Issues.Tests.Shared.Fixtures;
using Xunit;

namespace Spirebyte.Services.Issues.Tests.Integration.Commands;

[Collection("Spirebyte collection")]
public class CreateIssuesTests : IDisposable
{
    private const string Exchange = "issues";
    private readonly ICommandHandler<CreateIssue> _commandHandler;
    private readonly MongoDbFixture<IssueDocument, string> _issuesMongoDbFixture;
    private readonly MongoDbFixture<ProjectDocument, string> _projectsMongoDbFixture;
    private readonly RabbitMqFixture _rabbitMqFixture;
    private readonly MongoDbFixture<UserDocument, Guid> _usersMongoDbFixture;

    public CreateIssuesTests(SpirebyteApplicationFactory<Program> factory)
    {
        _rabbitMqFixture = new RabbitMqFixture();
        _issuesMongoDbFixture = new MongoDbFixture<IssueDocument, string>("issues");
        _projectsMongoDbFixture = new MongoDbFixture<ProjectDocument, string>("projects");
        _usersMongoDbFixture = new MongoDbFixture<UserDocument, Guid>("users");
        factory.Server.AllowSynchronousIO = true;
        _commandHandler = factory.Services.GetRequiredService<ICommandHandler<CreateIssue>>();
    }

    public void Dispose()
    {
        _issuesMongoDbFixture.Dispose();
        _projectsMongoDbFixture.Dispose();
        _usersMongoDbFixture.Dispose();
    }


    [Fact]
    public async Task create_issue_command_should_add_issue_with_given_data_to_database()
    {
        var projectId = "projectkey";
        var epicId = string.Empty;
        var issueId = string.Empty;
        var title = "Title";
        var description = "description";
        var type = IssueType.Story;
        var status = IssueStatus.TODO;
        var storypoints = 0;

        var expectedIssueId = $"{projectId}-1";

        var project = new Project(projectId);
        await _projectsMongoDbFixture.InsertAsync(project.AsDocument());


        var command = new CreateIssue(issueId, type, status, title, description, storypoints, projectId, epicId, null,
            null, DateTime.Now);

        // Check if exception is thrown

        await _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().NotThrowAsync();


        var issue = await _issuesMongoDbFixture.GetAsync(expectedIssueId);

        issue.Should().NotBeNull();
        issue.Id.Should().Be(expectedIssueId);
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
        var projectId = "projectKey";
        var epicId = "epicKey";
        var issueId = "issueKey";
        var title = "Title";
        var description = "description";
        var type = IssueType.Story;
        var status = IssueStatus.TODO;
        var storypoints = 0;

        var command = new CreateIssue(issueId, type, status, title, description, storypoints, projectId, epicId, null,
            null, DateTime.Now);

        // Check if exception is thrown

        await _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().ThrowAsync<ProjectNotFoundException>();
    }

    [Fact]
    public async Task create_issue_command_fails_when_epic_does_not_exist()
    {
        var projectId = "projectKey";
        var epicId = "epicKey";
        var issueId = "issueKey";
        var title = "Title";
        var description = "description";
        var type = IssueType.Story;
        var status = IssueStatus.TODO;
        var storypoints = 0;

        var project = new Project(projectId);
        await _projectsMongoDbFixture.InsertAsync(project.AsDocument());


        var command = new CreateIssue(issueId, type, status, title, description, storypoints, projectId, epicId, null,
            null, DateTime.Now);

        // Check if exception is thrown

        await _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().ThrowAsync<EpicNotFoundException>();
    }
}