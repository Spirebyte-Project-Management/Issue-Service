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
using Spirebyte.Services.Issues.Application.Issues.Services.Interfaces;
using Spirebyte.Services.Issues.Core.Constants;
using Spirebyte.Services.Issues.Core.Entities;
using Spirebyte.Services.Issues.Core.Enums;
using Spirebyte.Services.Issues.Core.Repositories;

namespace Spirebyte.Services.Issues.Application.Issues.Commands.Handlers;

internal sealed class UpdateIssueHandler : ICommandHandler<UpdateIssue>
{
    private readonly IContextAccessor _contextAccessor;
    private readonly IHistoryService _historyService;
    private readonly IIssueRepository _issueRepository;
    private readonly ILogger<UpdateIssueHandler> _logger;
    private readonly IMessageBroker _messageBroker;
    private readonly IProjectsApiHttpClient _projectsApiHttpClient;

    public UpdateIssueHandler(IIssueRepository issueRepository, ILogger<UpdateIssueHandler> logger,
        IMessageBroker messageBroker, IHistoryService historyService, IProjectsApiHttpClient projectsApiHttpClient,
        IContextAccessor contextAccessor)
    {
        _issueRepository = issueRepository;
        _logger = logger;
        _messageBroker = messageBroker;
        _historyService = historyService;
        _projectsApiHttpClient = projectsApiHttpClient;
        _contextAccessor = contextAccessor;
    }

    public async Task HandleAsync(UpdateIssue command, CancellationToken cancellationToken = default)
    {
        var issue = await _issueRepository.GetAsync(command.Id);
        if (issue is null) throw new IssueNotFoundException(command.Id);

        if (!string.IsNullOrWhiteSpace(command.EpicId) && !await _issueRepository.ExistsAsync(command.EpicId))
            throw new EpicNotFoundException(command.EpicId);

        if (!await _projectsApiHttpClient.HasPermission(IssuePermissionKeys.EditIssues, _contextAccessor.Context.GetUserId(),
                issue.ProjectId)) throw new ActionNotAllowedException();

        var newIssue = new Issue(issue.Id, command.Type, command.Status, command.Title, command.Description,
            command.StoryPoints, issue.ProjectId, command.EpicId, issue.SprintId, command.Assignees,
            command.LinkedIssues, issue.CreatedAt);
        await _issueRepository.UpdateAsync(newIssue);

        _logger.LogInformation($"Updated issue with id: {issue.Id}.");
        await _messageBroker.SendAsync(new IssueUpdated(newIssue, issue), cancellationToken);
        await _historyService.SaveHistory(issue, newIssue, HistoryTypes.Updated);
    }
}