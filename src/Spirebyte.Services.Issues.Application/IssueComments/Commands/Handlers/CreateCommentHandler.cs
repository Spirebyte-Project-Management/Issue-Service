using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Spirebyte.Services.Issues.Application.Contexts;
using Spirebyte.Services.Issues.Application.Exceptions;
using Spirebyte.Services.Issues.Application.IssueComments.Events;
using Spirebyte.Services.Issues.Application.Issues.Exceptions;
using Spirebyte.Services.Issues.Application.Services.Interfaces;
using Spirebyte.Services.Issues.Core.Entities;
using Spirebyte.Services.Issues.Core.Repositories;

namespace Spirebyte.Services.Issues.Application.IssueComments.Commands.Handlers;

internal sealed class CreateCommentHandler : ICommandHandler<CreateComment>
{
    private readonly IAppContext _appContext;
    private readonly ICommentRepository _commentRepository;
    private readonly IIssueRepository _issueRepository;
    private readonly IMessageBroker _messageBroker;
    private readonly IProjectRepository _projectRepository;

    public CreateCommentHandler(IProjectRepository projectRepository, IIssueRepository issueRepository,
        ICommentRepository commentRepository, IMessageBroker messageBroker, IAppContext appContext)
    {
        _projectRepository = projectRepository;
        _issueRepository = issueRepository;
        _commentRepository = commentRepository;
        _messageBroker = messageBroker;
        _appContext = appContext;
    }

    public async Task HandleAsync(CreateComment command, CancellationToken cancellationToken = default)
    {
        if (!await _projectRepository.ExistsAsync(command.ProjectId))
            throw new ProjectNotFoundException(command.ProjectId);

        if (!await _issueRepository.ExistsAsync(command.IssueId)) throw new IssueNotFoundException(command.IssueId);

        if (command.AuthorId != _appContext.Identity.Id && !_appContext.Identity.IsAdmin())
            throw new InvalidRoleException(_appContext.Identity.Id, _appContext.Identity.Role, "Admin");

        var commentCount = await _commentRepository.GetCommentCountOfIssue(command.IssueId);
        var commentId = $"{command.IssueId}-{commentCount + 1}";

        var comment = new Comment(commentId, command.IssueId, command.ProjectId, command.AuthorId, command.Body,
            DateTime.Now, new List<Reaction>());
        await _commentRepository.AddAsync(comment);

        await _messageBroker.PublishAsync(new CommentCreated(commentId, comment.IssueId, comment.ProjectId));
    }
}