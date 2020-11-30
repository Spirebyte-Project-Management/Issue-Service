using Convey.CQRS.Commands;
using Microsoft.Extensions.Logging;
using Spirebyte.Services.Issues.Application.Exceptions;
using Spirebyte.Services.Issues.Core.Entities;
using Spirebyte.Services.Issues.Core.Repositories;
using System.Threading.Tasks;

namespace Spirebyte.Services.Issues.Application.Commands.Handlers
{
    internal sealed class UpdateIssueHandler : ICommandHandler<UpdateIssue>
    {
        private readonly IIssueRepository _issueRepository;
        private readonly ILogger<UpdateIssueHandler> _logger;

        public UpdateIssueHandler(IIssueRepository issueRepository, ILogger<UpdateIssueHandler> logger)
        {
            _issueRepository = issueRepository;
            _logger = logger;
        }

        public async Task HandleAsync(UpdateIssue command)
        {
            var issue = await _issueRepository.GetAsync(command.Key);
            if (issue is null)
            {
                throw new IssueNotFoundException(command.IssueId);
            }


            issue = new Issue(issue.Id, issue.Key, command.Type, command.Status, command.Title, command.Description, command.StoryPoints, issue.ProjectId, command.Assignees, command.LinkedIssues, issue.CreatedAt);
            await _issueRepository.UpdateAsync(issue);

            _logger.LogInformation($"Updated issue with id: {issue.Id}.");

        }
    }
}
