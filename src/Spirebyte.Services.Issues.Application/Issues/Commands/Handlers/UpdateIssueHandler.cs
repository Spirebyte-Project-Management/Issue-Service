﻿using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Microsoft.Extensions.Logging;
using Spirebyte.Services.Issues.Application.Clients.Interfaces;
using Spirebyte.Services.Issues.Application.Exceptions;
using Spirebyte.Services.Issues.Application.Issues.Events;
using Spirebyte.Services.Issues.Application.Issues.Exceptions;
using Spirebyte.Services.Issues.Application.Issues.Services.Interfaces;
using Spirebyte.Services.Issues.Application.Services.Interfaces;
using Spirebyte.Services.Issues.Core.Constants;
using Spirebyte.Services.Issues.Core.Entities;
using Spirebyte.Services.Issues.Core.Enums;
using Spirebyte.Services.Issues.Core.Repositories;
using Spirebyte.Shared.Contexts.Interfaces;

namespace Spirebyte.Services.Issues.Application.Issues.Commands.Handlers;

internal sealed class UpdateIssueHandler : ICommandHandler<UpdateIssue>
{
    private readonly IAppContext _appContext;
    private readonly IHistoryService _historyService;
    private readonly IIssueRepository _issueRepository;
    private readonly ILogger<UpdateIssueHandler> _logger;
    private readonly IMessageBroker _messageBroker;
    private readonly IProjectsApiHttpClient _projectsApiHttpClient;

    public UpdateIssueHandler(IIssueRepository issueRepository, ILogger<UpdateIssueHandler> logger,
        IMessageBroker messageBroker, IHistoryService historyService, IProjectsApiHttpClient projectsApiHttpClient,
        IAppContext appContext)
    {
        _issueRepository = issueRepository;
        _logger = logger;
        _messageBroker = messageBroker;
        _historyService = historyService;
        _projectsApiHttpClient = projectsApiHttpClient;
        _appContext = appContext;
    }

    public async Task HandleAsync(UpdateIssue command, CancellationToken cancellationToken = default)
    {
        var issue = await _issueRepository.GetAsync(command.Id);
        if (issue is null) throw new IssueNotFoundException(command.Id);

        if (!string.IsNullOrWhiteSpace(command.EpicId) && !await _issueRepository.ExistsAsync(command.EpicId))
            throw new EpicNotFoundException(command.EpicId);

        if (!await _projectsApiHttpClient.HasPermission(IssuePermissionKeys.EditIssues, _appContext.Identity.Id,
                issue.ProjectId)) throw new ActionNotAllowedException();

        var newIssue = new Issue(issue.Id, command.Type, command.Status, command.Title, command.Description,
            command.StoryPoints, issue.ProjectId, command.EpicId, issue.SprintId, command.Assignees,
            command.LinkedIssues, issue.CreatedAt);
        await _issueRepository.UpdateAsync(newIssue);

        _logger.LogInformation($"Updated issue with id: {issue.Id}.");
        await _messageBroker.PublishAsync(new IssueUpdated(newIssue, issue));
        await _historyService.SaveHistory(issue, newIssue, HistoryTypes.Updated);
    }
}