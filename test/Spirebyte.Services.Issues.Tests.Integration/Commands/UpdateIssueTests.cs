using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Issues.API;
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
public class UpdateIssueTests : IDisposable
{
    private const string Exchange = "issues";
    private readonly ICommandHandler<UpdateIssue> _commandHandler;
    private readonly MongoDbFixture<IssueDocument, string> _issuesMongoDbFixture;
    private readonly MongoDbFixture<ProjectDocument, string> _projectsMongoDbFixture;
    private readonly RabbitMqFixture _rabbitMqFixture;
    private readonly MongoDbFixture<UserDocument, Guid> _usersMongoDbFixture;

    public UpdateIssueTests(SpirebyteApplicationFactory<Program> factory)
    {
        _rabbitMqFixture = new RabbitMqFixture();
        _issuesMongoDbFixture = new MongoDbFixture<IssueDocument, string>("issues");
        _projectsMongoDbFixture = new MongoDbFixture<ProjectDocument, string>("projects");
        _usersMongoDbFixture = new MongoDbFixture<UserDocument, Guid>("users");
        factory.Server.AllowSynchronousIO = true;
        _commandHandler = factory.Services.GetRequiredService<ICommandHandler<UpdateIssue>>();
    }

    public void Dispose()
    {
        _issuesMongoDbFixture.Dispose();
        _projectsMongoDbFixture.Dispose();
        _usersMongoDbFixture.Dispose();
    }


    [Fact]
    public async Task update_issue_command_should_update_issue_with_given_data_to()
    {
        var projectId = "projectKey";
        var epicId = string.Empty;
        var issueId = "issueKey";
        var sprintId = string.Empty;
        var title = "Title";
        var updatedTitle = "UpdatedTitle";
        var description = "description";
        var updatedDescription = "updatedDescription";
        var type = IssueType.Story;
        var status = IssueStatus.TODO;
        var storypoints = 0;

        var issue = new Issue(issueId, type, status, title, description, storypoints, projectId, epicId, sprintId, null,
            null, DateTime.Now);
        await _issuesMongoDbFixture.InsertAsync(issue.AsDocument());

        var command = new UpdateIssue(issueId, type, status, updatedTitle, updatedDescription, storypoints, epicId,
            null, null);

        // Check if exception is thrown

        await _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().NotThrowAsync();


        var updatedIssue = await _issuesMongoDbFixture.GetAsync(command.Id);

        updatedIssue.Should().NotBeNull();
        updatedIssue.Id.Should().Be(issueId);
        updatedIssue.Title.Should().Be(updatedTitle);
        updatedIssue.Description.Should().Be(updatedDescription);
    }

    [Fact]
    public async Task update_issue_command_fails_when_issue_with_key_does_not_exist()
    {
        var projectId = "projectKey";
        var epicId = "epicKey";
        var issueId = "issueKey";
        var title = "Title";
        var description = "description";
        var type = IssueType.Story;
        var status = IssueStatus.TODO;
        var storypoints = 0;

        var command = new UpdateIssue(issueId, type, status, title, description, storypoints, epicId, null, null);

        // Check if exception is thrown

        await _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().ThrowAsync<IssueNotFoundException>();
    }

    [Fact]
    public async Task update_issue_command_fails_when_epic_does_not_exist()
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


        var issue = new Issue(issueId, type, status, title, description, storypoints, projectId, string.Empty, sprintId,
            null, null, DateTime.Now);
        await _issuesMongoDbFixture.InsertAsync(issue.AsDocument());

        var command = new UpdateIssue(issueId, type, status, title, description, storypoints, epicId, null, null);

        // Check if exception is thrown

        await _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().ThrowAsync<EpicNotFoundException>();
    }
}