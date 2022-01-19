﻿using Convey.CQRS.Events;

namespace Spirebyte.Services.Issues.Application.Events;

[Contract]
public class CommentDeleted : IEvent
{
    public CommentDeleted(string commentId, string issueId)
    {
        CommentId = commentId;
        IssueId = issueId;
    }

    public string CommentId { get; }
    public string IssueId { get; }
}