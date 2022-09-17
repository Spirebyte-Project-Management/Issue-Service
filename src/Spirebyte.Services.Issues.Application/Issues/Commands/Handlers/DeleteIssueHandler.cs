using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Spirebyte.Framework.Contexts;
using Spirebyte.Framework.Messaging.Brokers;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Services.Issues.Application.Clients.Interfaces;
using Spirebyte.Services.Issues.Application.Exceptions;
using Spirebyte.Services.Issues.Application.Issues.Events;
using Spirebyte.Services.Issues.Application.Issues.Exceptions;
using Spirebyte.Services.Issues.Core.Constants;
using Spirebyte.Services.Issues.Core.Repositories;

namespace Spirebyte.Services.Issues.Application.Issues.Commands.Handlers;

internal sealed class DeleteIssueHandler : ICommandHandler<DeleteIssue>
{
    private readonly IContextAccessor _contextAccessor;
    private readonly IIssueRepository _issueRepository;
    private readonly ILogger<DeleteIssueHandler> _logger;
    private readonly IMessageBroker _messageBroker;
    private readonly IProjectsApiHttpClient _projectsApiHttpClient;

    public DeleteIssueHandler(IIssueRepository issueRepository, ILogger<DeleteIssueHandler> logger,
        IMessageBroker messageBroker, IProjectsApiHttpClient projectsApiHttpClient, IContextAccessor contextAccessor)
    {
        _issueRepository = issueRepository;
        _logger = logger;
        _messageBroker = messageBroker;
        _projectsApiHttpClient = projectsApiHttpClient;
        _contextAccessor = contextAccessor;
    }

    public async Task HandleAsync(DeleteIssue command, CancellationToken cancellationToken = default)
    {
        var issue = await _issueRepository.GetAsync(command.Id);
        if (issue is null) throw new IssueNotFoundException(command.Id);

        if (!await _projectsApiHttpClient.HasPermission(IssuePermissionKeys.DeleteIssues, _contextAccessor.Context.GetUserId(),
                issue.ProjectId)) throw new ActionNotAllowedException();

        await _issueRepository.DeleteAsync(issue.Id);

        _logger.LogInformation($"Deleted issue with id: {issue.Id}.");
        await _messageBroker.SendAsync(new IssueDeleted(issue), cancellationToken);
    }
}