using Convey.CQRS.Commands;
using NSubstitute;
using Spirebyte.Services.Issues.Application;
using Spirebyte.Services.Issues.Application.Clients.Interfaces;
using Spirebyte.Services.Issues.Application.Commands;
using Spirebyte.Services.Issues.Application.Commands.Handlers;
using Spirebyte.Services.Issues.Application.Services.Interfaces;
using Spirebyte.Services.Issues.Core.Enums;
using Spirebyte.Services.Issues.Core.Repositories;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Spirebyte.Services.Issues.Tests.Unit.Application
{
    public class CreateIssueTests
    {
        private Task Act(CreateIssue command) => _handler.HandleAsync(command);

        [Fact]
        public async Task given_valid_input_create_issue_should_succeed()
        {
            var projectId = "projectkey";
            var epicId = string.Empty;
            var issueId = string.Empty;
            var title = "Title";
            var description = "description";
            var type = IssueType.Story;
            var status = IssueStatus.TODO;
            var storypoints = 0;

            _projectRepository.ExistsAsync(projectId).Returns(true);
            _projectsApiHttpClient.HasPermission(default, default, default).ReturnsForAnyArgs(true);

            var command = new CreateIssue(issueId, type, status, title, description, storypoints, projectId, epicId, null, null, DateTime.Now);
            await Act(command);

            await _projectRepository.Received().ExistsAsync(projectId);
            await _issueRepository.ReceivedWithAnyArgs().AddAsync(default);
        }

        private readonly IProjectRepository _projectRepository;
        private readonly IIssueRepository _issueRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IHistoryService _historyService;
        private readonly IProjectsApiHttpClient _projectsApiHttpClient;
        private readonly IAppContext _appContext;
        private readonly ICommandHandler<CreateIssue> _handler;

        public CreateIssueTests()
        {
            _projectRepository = Substitute.For<IProjectRepository>();
            _issueRepository = Substitute.For<IIssueRepository>();
            _messageBroker = Substitute.For<IMessageBroker>();
            _historyService = Substitute.For<IHistoryService>();
            _projectsApiHttpClient = Substitute.For<IProjectsApiHttpClient>();
            _appContext = Substitute.For<IAppContext>();

            _handler = new CreateIssueHandler(_projectRepository, _issueRepository, _messageBroker, _historyService, _projectsApiHttpClient, _appContext);
        }
    }
}
