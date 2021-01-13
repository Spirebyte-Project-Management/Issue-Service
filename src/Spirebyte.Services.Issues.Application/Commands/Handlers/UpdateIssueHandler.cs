using Convey.CQRS.Commands;
using Microsoft.Extensions.Logging;
using Spirebyte.Services.Issues.Application.Exceptions;
using Spirebyte.Services.Issues.Core.Entities;
using Spirebyte.Services.Issues.Core.Repositories;
using System.Threading.Tasks;
using Spirebyte.Services.Issues.Application.Events;
using Spirebyte.Services.Issues.Application.Services.Interfaces;

namespace Spirebyte.Services.Issues.Application.Commands.Handlers
{
    internal sealed class UpdateIssueHandler : ICommandHandler<UpdateIssue>
    {
        private readonly IIssueRepository _issueRepository;
        private readonly ILogger<UpdateIssueHandler> _logger;
        private readonly IMessageBroker _messageBroker;

        public UpdateIssueHandler(IIssueRepository issueRepository, ILogger<UpdateIssueHandler> logger, IMessageBroker messageBroker)
        {
            _issueRepository = issueRepository;
            _logger = logger;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(UpdateIssue command)
        {
            var issue = await _issueRepository.GetAsync(command.IssueId);
            if (issue is null)
            {
                throw new IssueNotFoundException(command.IssueId);
            }

            if (!string.IsNullOrWhiteSpace(command.EpicId) && !(await _issueRepository.ExistsAsync(command.EpicId)))
            {
                throw new EpicNotFoundException(command.EpicId);
            }

            issue = new Issue(issue.Id, command.Type, command.Status, command.Title, command.Description, command.StoryPoints, issue.ProjectId, command.EpicId, issue.SprintId, command.Assignees, command.LinkedIssues, issue.CreatedAt);
            await _issueRepository.UpdateAsync(issue);

            _logger.LogInformation($"Updated issue with id: {issue.Id}.");
            await _messageBroker.PublishAsync(new IssueUpdated(issue.Id));
        }
    }
}
