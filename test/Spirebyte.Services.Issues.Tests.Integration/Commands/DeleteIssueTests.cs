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
using Spirebyte.Services.Issues.Core.Repositories;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents.Mappers;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Repositories;
using Spirebyte.Services.Issues.Tests.Shared.MockData.Entities;
using Xunit;

namespace Spirebyte.Services.Issues.Tests.Integration.Commands;

public class DeleteIssueTests : TestBase
{
    private readonly TestMessageBroker _messageBroker;

    private readonly ICommandHandler<DeleteIssue> _commandHandler;

    private readonly IIssueRepository _issueRepository;
    private readonly ILogger<DeleteIssueHandler> _logger;
    private readonly IProjectsApiHttpClient _projectsApiHttpClient;
    private readonly IContextAccessor _contextAccessor;

    public DeleteIssueTests(
        MongoDbFixture<CommentDocument, string> commentsMongoDbFixture,
        MongoDbFixture<HistoryDocument, Guid> historyMongoDbFixture,
        MongoDbFixture<IssueDocument, string> issuesMongoDbFixture,
        MongoDbFixture<ProjectDocument, string> projectsMongoDbFixture,
        MongoDbFixture<UserDocument, Guid> usersMongoDbFixture) : base(commentsMongoDbFixture, historyMongoDbFixture, issuesMongoDbFixture, projectsMongoDbFixture, usersMongoDbFixture)
    {
        _messageBroker = new TestMessageBroker();

        _issueRepository = new IssueRepository(IssuesMongoDbFixture);
        _logger = Substitute.For<ILogger<DeleteIssueHandler>>();

        _projectsApiHttpClient = Substitute.For<IProjectsApiHttpClient>();
        _projectsApiHttpClient.HasPermission(default, default, default).ReturnsForAnyArgs(true);
        
        _contextAccessor = Substitute.For<IContextAccessor>();
        _contextAccessor.Context.Returns(new Context("some-activity-id", "some-trace-id", "some-correlation-id", "some-message-id", "some-causation-id", Guid.NewGuid().ToString()));

        _commandHandler = new DeleteIssueHandler(_issueRepository, _logger, _messageBroker, _projectsApiHttpClient, _contextAccessor);
    }
    
    [Fact]
    public async Task delete_issue_command_should_remove_issue_with_given_key()
    {
        var fakedIssue = IssueFaker.Instance.Generate();
        await IssuesMongoDbFixture.AddAsync(fakedIssue.AsDocument());

        var command = new DeleteIssue(fakedIssue.Id);

        // Check if exception is thrown
        await _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().NotThrowAsync();
        
        var deletedIssue = await IssuesMongoDbFixture.GetAsync(fakedIssue.Id);
        deletedIssue.Should().BeNull();
    }

    [Fact]
    public async Task delete_issue_command_fails_when_issue_with_key_does_not_exist()
    {
        var fakedIssue = IssueFaker.Instance.Generate();

        var command = new DeleteIssue(fakedIssue.Id);

        // Check if exception is thrown
        await _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().ThrowAsync<IssueNotFoundException>();
    }
}