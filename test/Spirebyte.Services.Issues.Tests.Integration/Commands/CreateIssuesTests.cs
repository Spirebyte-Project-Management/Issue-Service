using System;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Spirebyte.Framework.Contexts;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Framework.Tests.Shared.Fixtures;
using Spirebyte.Framework.Tests.Shared.Infrastructure;
using Spirebyte.Services.Issues.Application.Clients.Interfaces;
using Spirebyte.Services.Issues.Application.Exceptions;
using Spirebyte.Services.Issues.Application.Issues.Commands;
using Spirebyte.Services.Issues.Application.Issues.Commands.Handlers;
using Spirebyte.Services.Issues.Application.Issues.Events;
using Spirebyte.Services.Issues.Application.Issues.Exceptions;
using Spirebyte.Services.Issues.Application.Issues.Services.Interfaces;
using Spirebyte.Services.Issues.Core.Entities;
using Spirebyte.Services.Issues.Core.Repositories;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Repositories;
using Spirebyte.Services.Issues.Tests.Shared.MockData.Entities;
using Xunit;

namespace Spirebyte.Services.Issues.Tests.Integration.Commands;

public class CreateIssuesTests : TestBase
{
    private readonly TestMessageBroker _messageBroker;

    private readonly ICommandHandler<CreateIssue> _commandHandler;

    private readonly IProjectRepository _projectRepository;
    private readonly IIssueRepository _issueRepository;
    private readonly IHistoryService _historyService;
    private readonly IProjectsApiHttpClient _projectsApiHttpClient;
    private readonly IContextAccessor _contextAccessor;
    private readonly IIssueRequestStorage _issueRequestStorage;

    public CreateIssuesTests(
        MongoDbFixture<CommentDocument, string> commentsMongoDbFixture,
        MongoDbFixture<HistoryDocument, Guid> historyMongoDbFixture,
        MongoDbFixture<IssueDocument, string> issuesMongoDbFixture,
        MongoDbFixture<ProjectDocument, string> projectsMongoDbFixture,
        MongoDbFixture<UserDocument, Guid> usersMongoDbFixture) : base(commentsMongoDbFixture, historyMongoDbFixture, issuesMongoDbFixture, projectsMongoDbFixture, usersMongoDbFixture)
    {
        _messageBroker = new TestMessageBroker();

        _projectRepository = new ProjectRepository(ProjectsMongoDbFixture);
        _issueRepository = new IssueRepository(IssuesMongoDbFixture);

        _historyService = Substitute.For<IHistoryService>();
        _projectsApiHttpClient = Substitute.For<IProjectsApiHttpClient>();
        _contextAccessor = Substitute.For<IContextAccessor>();
        _contextAccessor.Context.Returns(new Context("some-activity-id", "some-trace-id", "some-correlation-id", "some-message-id", "some-causation-id", Guid.NewGuid().ToString()));
        _issueRequestStorage = Substitute.For<IIssueRequestStorage>();
        
        _commandHandler = new CreateIssueHandler(_projectRepository, _issueRepository, _messageBroker, _historyService, _projectsApiHttpClient, _contextAccessor, _issueRequestStorage);
    }

    [Fact]
    public async Task create_issue_command_should_add_issue_with_given_data_to_database()
    {
        var fakedIssue = IssueFaker.Instance.Generate();

        await _projectRepository.AddAsync(new Project(fakedIssue.ProjectId));

        _projectsApiHttpClient.HasPermission(default, default, default).ReturnsForAnyArgs(true);

        var command = new CreateIssue(fakedIssue.Type, fakedIssue.Status, fakedIssue.Title, fakedIssue.Description,
            fakedIssue.StoryPoints, fakedIssue.ProjectId, fakedIssue.EpicId, fakedIssue.Assignees,
            fakedIssue.LinkedIssues, DateTime.Now);

        var currentIssues = await IssuesMongoDbFixture.FindAsync(r => r.ProjectId == fakedIssue.ProjectId);

        _issueRequestStorage.SetIssue(
            Arg.Do<Guid>(r => { r.Should().Be(command.ReferenceId); }),
            Arg.Do<Issue>(issue =>
            {
                issue.Should().NotBeNull();
                issue.Id.Should().Be($"{fakedIssue.ProjectId}-{currentIssues.Count + 1}");
                issue.Type.Should().Be(fakedIssue.Type);
                issue.Status.Should().Be(fakedIssue.Status);
                issue.Title.Should().Be(fakedIssue.Title);
                issue.Description.Should().Be(fakedIssue.Description);
                issue.StoryPoints.Should().Be(fakedIssue.StoryPoints);
                issue.ProjectId.Should().Be(fakedIssue.ProjectId);
                issue.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMinutes(1));
            })
        );
        
        // Check if exception is thrown
        await _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().NotThrowAsync();

        _messageBroker.Events.Should().NotBeEmpty();
        _messageBroker.Events.Count.Should().Be(1);
        var @event = _messageBroker.Events[0];
        @event.Should().BeOfType<IssueCreated>();

        var issues = await IssuesMongoDbFixture.FindAsync(r => r.Title == fakedIssue.Title);
        issues.Should().NotBeEmpty();
    }

    [Fact]
    public async Task create_issue_command_fails_when_project_does_not_exist()
    {
        var fakedIssue = IssueFaker.Instance.Generate();
        
        _projectsApiHttpClient.HasPermission(default, default, default).ReturnsForAnyArgs(true);

        var command = new CreateIssue(fakedIssue.Type, fakedIssue.Status, fakedIssue.Title, fakedIssue.Description,
            fakedIssue.StoryPoints, fakedIssue.ProjectId, fakedIssue.EpicId, fakedIssue.Assignees,
            fakedIssue.LinkedIssues, DateTime.Now);

        // Check if exception is thrown
        await _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().ThrowAsync<ProjectNotFoundException>();
    }

    [Fact]
    public async Task create_issue_command_fails_when_epic_does_not_exist()
    {
        var fakedIssue = IssueFaker.Instance.Generate();
        
        await _projectRepository.AddAsync(new Project(fakedIssue.ProjectId));
        
        _projectsApiHttpClient.HasPermission(default, default, default).ReturnsForAnyArgs(true);

        var command = new CreateIssue(fakedIssue.Type, fakedIssue.Status, fakedIssue.Title, fakedIssue.Description,
            fakedIssue.StoryPoints, fakedIssue.ProjectId, fakedIssue.Id, fakedIssue.Assignees,
            fakedIssue.LinkedIssues, DateTime.Now);

        // Check if exception is thrown
        await _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().ThrowAsync<EpicNotFoundException>();
    }
}