using Convey.CQRS.Commands;
using Microsoft.Extensions.Logging;
using Spirebyte.Services.Issues.Application.Exceptions;
using Spirebyte.Services.Issues.Core.Repositories;
using System.Threading.Tasks;
using Spirebyte.Services.Issues.Application.Events;
using Spirebyte.Services.Issues.Application.Services.Interfaces;

namespace Spirebyte.Services.Issues.Application.Commands.Handlers
{
    internal sealed class DeleteIssueHandler : ICommandHandler<DeleteIssue>
    {
        private readonly IIssueRepository _issueRepository;
        private readonly ILogger<DeleteIssueHandler> _logger;
        private readonly IMessageBroker _messageBroker;

        public DeleteIssueHandler(IIssueRepository issueRepository, ILogger<DeleteIssueHandler> logger, IMessageBroker messageBroker)
        {
            _issueRepository = issueRepository;
            _logger = logger;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(DeleteIssue command)
        {
            var issue = await _issueRepository.GetAsync(command.IssueId);
            if (issue is null)
            {
                throw new IssueNotFoundException(command.IssueId);
            }

            await _issueRepository.DeleteAsync(issue.Id);

            _logger.LogInformation($"Deleted issue with id: {issue.Id}.");
            await _messageBroker.PublishAsync(new IssueDeleted(issue.Id, issue.ProjectId));
        }
    }
}
