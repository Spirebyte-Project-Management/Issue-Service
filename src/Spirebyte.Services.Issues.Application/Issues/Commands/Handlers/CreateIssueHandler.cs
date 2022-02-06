using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Spirebyte.Services.Issues.Application.Clients.Interfaces;
using Spirebyte.Services.Issues.Application.Contexts;
using Spirebyte.Services.Issues.Application.Exceptions;
using Spirebyte.Services.Issues.Application.Issues.Events;
using Spirebyte.Services.Issues.Application.Issues.Exceptions;
using Spirebyte.Services.Issues.Application.Issues.Services.Interfaces;
using Spirebyte.Services.Issues.Application.Services.Interfaces;
using Spirebyte.Services.Issues.Core.Constants;
using Spirebyte.Services.Issues.Core.Entities;
using Spirebyte.Services.Issues.Core.Enums;
using Spirebyte.Services.Issues.Core.Repositories;

namespace Spirebyte.Services.Issues.Application.Issues.Commands.Handlers;

// Simple wrapper
internal sealed class CreateIssueHandler : ICommandHandler<CreateIssue>
{
    private readonly IAppContext _appContext;
    private readonly IHistoryService _historyService;
    private readonly IIssueRepository _issueRepository;
    private readonly IIssueRequestStorage _issueRequestStorage;
    private readonly IMessageBroker _messageBroker;
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectsApiHttpClient _projectsApiHttpClient;

    public CreateIssueHandler(IProjectRepository projectRepository, IIssueRepository issueRepository,
        IMessageBroker messageBroker, IHistoryService historyService, IProjectsApiHttpClient projectsApiHttpClient,
        IAppContext appContext, IIssueRequestStorage issueRequestStorage)
    {
        _projectRepository = projectRepository;
        _issueRepository = issueRepository;
        _messageBroker = messageBroker;
        _historyService = historyService;
        _projectsApiHttpClient = projectsApiHttpClient;
        _appContext = appContext;
        _issueRequestStorage = issueRequestStorage;
    }

    public async Task HandleAsync(CreateIssue command, CancellationToken cancellationToken = default)
    {
        if (!await _projectRepository.ExistsAsync(command.ProjectId))
            throw new ProjectNotFoundException(command.ProjectId);

        if (!string.IsNullOrEmpty(command.EpicId) && !await _issueRepository.ExistsAsync(command.EpicId))
            throw new EpicNotFoundException(command.EpicId);

        if (!await _projectsApiHttpClient.HasPermission(IssuePermissionKeys.CreateIssues, _appContext.Identity.Id,
                command.ProjectId)) throw new ActionNotAllowedException();

        var issueCount = await _issueRepository.GetIssueCountOfProject(command.ProjectId);
        var issueId = $"{command.ProjectId}-{issueCount + 1}";


        var issue = new Issue(issueId, command.Type, command.Status, command.Title, command.Description,
            command.StoryPoints, command.ProjectId, command.EpicId, null, command.Assignees, command.LinkedIssues,
            command.CreatedAt);
        await _issueRepository.AddAsync(issue);
        await _messageBroker.PublishAsync(new IssueCreated(issue.Id, issue.ProjectId, issue.StoryPoints));

        await _historyService.SaveHistory(Issue.Empty, issue, HistoryTypes.Created);

        _issueRequestStorage.SetIssue(command.ReferenceId, issue);
    }
}