using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Microsoft.Extensions.Logging;
using Spirebyte.Services.Issues.Application.Exceptions;
using Spirebyte.Services.Issues.Core.Entities;
using Spirebyte.Services.Issues.Core.Repositories;

namespace Spirebyte.Services.Issues.Application.Commands.Handlers
{
    internal sealed class DeleteIssueHandler : ICommandHandler<DeleteIssue>
    {
        private readonly IIssueRepository _issueRepository;
        private readonly ILogger<DeleteIssueHandler> _logger;

        public DeleteIssueHandler(IIssueRepository issueRepository, ILogger<DeleteIssueHandler> logger)
        {
            _issueRepository = issueRepository;
            _logger = logger;
        }

        public async Task HandleAsync(DeleteIssue command)
        {
            var issue = await _issueRepository.GetAsync(command.Key);
            if (issue is null)
            {
                throw new IssueNotFoundException(command.Key);
            }


            await _issueRepository.DeleteAsync(issue.Id);

            _logger.LogInformation($"Deleted issue with id: {issue.Id}.");

        }
    }
}
