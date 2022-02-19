using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Microsoft.Extensions.Logging;
using Spirebyte.Services.Issues.Application.Clients.Interfaces;
using Spirebyte.Services.Issues.Application.Exceptions;
using Spirebyte.Services.Issues.Application.Issues.Events;
using Spirebyte.Services.Issues.Application.Issues.Exceptions;
using Spirebyte.Services.Issues.Application.Services.Interfaces;
using Spirebyte.Services.Issues.Core.Constants;
using Spirebyte.Services.Issues.Core.Repositories;
using Spirebyte.Shared.Contexts.Interfaces;

namespace Spirebyte.Services.Issues.Application.Issues.Commands.Handlers;

internal sealed class DeleteIssueHandler : ICommandHandler<DeleteIssue>
{
    private readonly IAppContext _appContext;
    private readonly IIssueRepository _issueRepository;
    private readonly ILogger<DeleteIssueHandler> _logger;
    private readonly IMessageBroker _messageBroker;
    private readonly IProjectsApiHttpClient _projectsApiHttpClient;

    public DeleteIssueHandler(IIssueRepository issueRepository, ILogger<DeleteIssueHandler> logger,
        IMessageBroker messageBroker, IProjectsApiHttpClient projectsApiHttpClient, IAppContext appContext)
    {
        _issueRepository = issueRepository;
        _logger = logger;
        _messageBroker = messageBroker;
        _projectsApiHttpClient = projectsApiHttpClient;
        _appContext = appContext;
    }

    public async Task HandleAsync(DeleteIssue command, CancellationToken cancellationToken = default)
    {
        var issue = await _issueRepository.GetAsync(command.Id);
        if (issue is null) throw new IssueNotFoundException(command.Id);

        if (!await _projectsApiHttpClient.HasPermission(IssuePermissionKeys.DeleteIssues, _appContext.Identity.Id,
                issue.ProjectId)) throw new ActionNotAllowedException();

        await _issueRepository.DeleteAsync(issue.Id);

        _logger.LogInformation($"Deleted issue with id: {issue.Id}.");
        await _messageBroker.PublishAsync(new IssueDeleted(issue.Id, issue.ProjectId));
    }
}