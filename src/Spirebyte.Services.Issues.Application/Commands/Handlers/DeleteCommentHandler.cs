using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Microsoft.Extensions.Logging;
using Spirebyte.Services.Issues.Application.Events;
using Spirebyte.Services.Issues.Application.Exceptions;
using Spirebyte.Services.Issues.Application.Services.Interfaces;
using Spirebyte.Services.Issues.Core.Repositories;

namespace Spirebyte.Services.Issues.Application.Commands.Handlers;

internal sealed class DeleteCommentHandler : ICommandHandler<DeleteComment>
{
    private readonly ICommentRepository _commentRepository;
    private readonly ILogger<DeleteCommentHandler> _logger;
    private readonly IMessageBroker _messageBroker;

    public DeleteCommentHandler(ICommentRepository commentRepository, ILogger<DeleteCommentHandler> logger,
        IMessageBroker messageBroker)
    {
        _commentRepository = commentRepository;
        _logger = logger;
        _messageBroker = messageBroker;
    }

    public async Task HandleAsync(DeleteComment command)
    {
        var comment = await _commentRepository.GetAsync(command.Id);
        if (comment is null) throw new CommentNotFoundException(command.Id);

        await _commentRepository.DeleteAsync(comment.Id);

        _logger.LogInformation($"Deleted comment with id: {comment.Id}.");
        await _messageBroker.PublishAsync(new CommentDeleted(comment.Id, comment.IssueId));
    }
}