using Convey.CQRS.Commands;
using Spirebyte.Services.Issues.Application.Events;
using Spirebyte.Services.Issues.Application.Exceptions;
using Spirebyte.Services.Issues.Application.Services.Interfaces;
using Spirebyte.Services.Issues.Core.Entities;
using Spirebyte.Services.Issues.Core.Repositories;
using System.Threading.Tasks;
using Spirebyte.Services.Issues.Core.Enums;

namespace Spirebyte.Services.Issues.Application.Commands.Handlers
{
    // Simple wrapper
    internal sealed class CreateIssueHandler : ICommandHandler<CreateIssue>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IIssueRepository _issueRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IHistoryService _historyService;

        public CreateIssueHandler(IProjectRepository projectRepository, IIssueRepository issueRepository,
            IMessageBroker messageBroker, IHistoryService historyService)
        {
            _projectRepository = projectRepository;
            _issueRepository = issueRepository;
            _messageBroker = messageBroker;
            _historyService = historyService;
        }

        public async Task HandleAsync(CreateIssue command)
        {
            if (!(await _projectRepository.ExistsAsync(command.ProjectId)))
            {
                throw new ProjectNotFoundException(command.ProjectId);
            }

            if (!string.IsNullOrEmpty(command.EpicId) && !(await _issueRepository.ExistsAsync(command.EpicId)))
            {
                throw new EpicNotFoundException(command.EpicId);
            }

            var issueCount = await _issueRepository.GetIssueCountOfProject(command.ProjectId);
            var issueId = $"{command.ProjectId}-{issueCount + 1}";


            var issue = new Issue(issueId, command.Type, command.Status, command.Title, command.Description, command.StoryPoints, command.ProjectId, command.EpicId, null, command.Assignees, command.LinkedIssues, command.CreatedAt);
            await _issueRepository.AddAsync(issue);
            await _messageBroker.PublishAsync(new IssueCreated(issue.Id, issue.ProjectId));

            await _historyService.SaveHistory(Issue.Empty, issue, HistoryTypes.Created);
        }
    }
}
