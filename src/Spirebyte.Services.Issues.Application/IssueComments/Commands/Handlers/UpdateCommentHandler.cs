using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Spirebyte.Framework.Messaging.Brokers;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Services.Issues.Application.Exceptions;
using Spirebyte.Services.Issues.Application.IssueComments.Events;
using Spirebyte.Services.Issues.Core.Entities;
using Spirebyte.Services.Issues.Core.Repositories;

namespace Spirebyte.Services.Issues.Application.IssueComments.Commands.Handlers;

internal sealed class UpdateCommentHandler : ICommandHandler<UpdateComment>
{
    private readonly ICommentRepository _commentRepository;
    private readonly ILogger<UpdateCommentHandler> _logger;
    private readonly IMessageBroker _messageBroker;

    public UpdateCommentHandler(ICommentRepository commentRepository, ILogger<UpdateCommentHandler> logger,
        IMessageBroker messageBroker)
    {
        _commentRepository = commentRepository;
        _logger = logger;
        _messageBroker = messageBroker;
    }

    public async Task HandleAsync(UpdateComment command, CancellationToken cancellationToken = default)
    {
        var comment = await _commentRepository.GetAsync(command.Id);
        if (comment is null) throw new CommentNotFoundException(command.Id);

        comment = new Comment(comment.Id, comment.IssueId, comment.ProjectId, comment.AuthorId, command.Body,
            comment.CreatedAt, DateTime.Now, comment.Reactions);
        await _commentRepository.UpdateAsync(comment);

        _logger.LogInformation($"Updated comment with id: {comment.Id}.");
        await _messageBroker.SendAsync(new CommentUpdated(comment.Id), cancellationToken);
    }
}