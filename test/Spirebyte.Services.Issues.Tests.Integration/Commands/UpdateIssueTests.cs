using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Spirebyte.Framework.Contexts;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Framework.Tests.Shared.Fixtures;
using Spirebyte.Framework.Tests.Shared.Infrastructure;
using Spirebyte.Services.Issues.Application.Clients.Interfaces;
using Spirebyte.Services.Issues.Application.Issues.Commands;
using Spirebyte.Services.Issues.Application.Issues.Commands.Handlers;
using Spirebyte.Services.Issues.Application.Issues.Exceptions;
using Spirebyte.Services.Issues.Application.Issues.Services.Interfaces;
using Spirebyte.Services.Issues.Core.Entities;
using Spirebyte.Services.Issues.Core.Enums;
using Spirebyte.Services.Issues.Core.Repositories;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents.Mappers;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Repositories;
using Spirebyte.Services.Issues.Tests.Shared.MockData.Entities;
using Xunit;

namespace Spirebyte.Services.Issues.Tests.Integration.Commands;

public class UpdateIssueTests : TestBase
{
    private readonly TestMessageBroker _messageBroker;

    private readonly ICommandHandler<UpdateIssue> _commandHandler;

    private readonly IIssueRepository _issueRepository;
    private readonly ILogger<UpdateIssueHandler> _logger;
    private readonly IHistoryService _historyService;
    private readonly IProjectsApiHttpClient _projectsApiHttpClient;
    private readonly IContextAccessor _contextAccessor;
    
    public UpdateIssueTests(
        MongoDbFixture<CommentDocument, string> commentsMongoDbFixture,
        MongoDbFixture<HistoryDocument, Guid> historyMongoDbFixture,
        MongoDbFixture<IssueDocument, string> issuesMongoDbFixture,
        MongoDbFixture<ProjectDocument, string> projectsMongoDbFixture,
        MongoDbFixture<UserDocument, Guid> usersMongoDbFixture) : base(commentsMongoDbFixture, historyMongoDbFixture, issuesMongoDbFixture, projectsMongoDbFixture, usersMongoDbFixture)
    {
        _messageBroker = new TestMessageBroker();

        _issueRepository = new IssueRepository(IssuesMongoDbFixture);
        _logger = Substitute.For<ILogger<UpdateIssueHandler>>();

        _historyService = Substitute.For<IHistoryService>();
        _projectsApiHttpClient = Substitute.For<IProjectsApiHttpClient>();
        _projectsApiHttpClient.HasPermission(default, default, default).ReturnsForAnyArgs(true);

        _contextAccessor = Substitute.For<IContextAccessor>();
        _contextAccessor.Context.Returns(new Context("some-activity-id", "some-trace-id", "some-correlation-id", "some-message-id", "some-causation-id", Guid.NewGuid().ToString()));

        _commandHandler = new UpdateIssueHandler(_issueRepository, _logger, _messageBroker, _historyService, _projectsApiHttpClient, _contextAccessor);
    }
    
    [Fact]
    public async Task update_issue_command_should_update_issue_with_given_data_to()
    {
        var initalFakedIssue = IssueFaker.Instance.Generate();
        var updatedFakedIssue = IssueFaker.Instance.Generate();
        
        await IssuesMongoDbFixture.AddAsync(initalFakedIssue.AsDocument());

        var command = new UpdateIssue(initalFakedIssue.Id, initalFakedIssue.Type, initalFakedIssue.Status, updatedFakedIssue.Title, updatedFakedIssue.Description, updatedFakedIssue.StoryPoints, initalFakedIssue.EpicId,
            initalFakedIssue.Assignees, initalFakedIssue.LinkedIssues);

        // Check if exception is thrown

        await _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().NotThrowAsync();


        var updatedIssue = await IssuesMongoDbFixture.GetAsync(command.Id);

        updatedIssue.Should().NotBeNull();
        updatedIssue.Title.Should().Be(updatedFakedIssue.Title);
        updatedIssue.Description.Should().Be(updatedFakedIssue.Description);
        updatedIssue.StoryPoints.Should().Be(updatedFakedIssue.StoryPoints);
    }

    [Fact]
    public async Task update_issue_command_fails_when_issue_with_key_does_not_exist()
    {
        var fakedIssue = IssueFaker.Instance.Generate();

        var command = new UpdateIssue(fakedIssue.Id, fakedIssue.Type, fakedIssue.Status, fakedIssue.Title,
            fakedIssue.Description, fakedIssue.StoryPoints, fakedIssue.EpicId, fakedIssue.Assignees,
            fakedIssue.LinkedIssues);

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
        await IssuesMongoDbFixture.AddAsync(issue.AsDocument());

        var command = new UpdateIssue(issueId, type, status, title, description, storypoints, epicId, null, null);

        // Check if exception is thrown

        await _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().ThrowAsync<EpicNotFoundException>();
    }
}