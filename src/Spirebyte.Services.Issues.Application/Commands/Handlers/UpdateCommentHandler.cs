using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Microsoft.Extensions.Logging;
using Spirebyte.Services.Issues.Application.Events;
using Spirebyte.Services.Issues.Application.Exceptions;
using Spirebyte.Services.Issues.Application.Services.Interfaces;
using Spirebyte.Services.Issues.Core.Entities;
using Spirebyte.Services.Issues.Core.Enums;
using Spirebyte.Services.Issues.Core.Repositories;

namespace Spirebyte.Services.Issues.Application.Commands.Handlers
{
    internal sealed class UpdateCommentHandler : ICommandHandler<UpdateComment>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly ILogger<UpdateCommentHandler> _logger;
        private readonly IMessageBroker _messageBroker;

        public UpdateCommentHandler(ICommentRepository commentRepository, ILogger<UpdateCommentHandler> logger, IMessageBroker messageBroker)
        {
            _commentRepository = commentRepository;
            _logger = logger;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(UpdateComment command)
        {
            var comment = await _commentRepository.GetAsync(command.Id);
            if (comment is null)
            {
                throw new CommentNotFoundException(command.Id);
            }

            comment = new Comment(comment.Id, comment.IssueId, comment.ProjectId, comment.AuthorId, command.Body, comment.CreatedAt, DateTime.Now,  comment.Reactions);
            await _commentRepository.UpdateAsync(comment);

            _logger.LogInformation($"Updated comment with id: {comment.Id}.");
            await _messageBroker.PublishAsync(new CommentUpdated(comment.Id));
        }
    }
}
