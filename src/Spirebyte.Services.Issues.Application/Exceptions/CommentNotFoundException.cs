﻿using Spirebyte.Services.Issues.Application.Exceptions.Base;

namespace Spirebyte.Services.Issues.Application.Exceptions;

public class CommentNotFoundException : AppException
{
    public CommentNotFoundException(string commentID) : base($"Comment with ID: '{commentID}' was not found.")
    {
        CommentId = commentID;
    }

    public override string Code { get; } = "comment_not_found";
    public string CommentId { get; }
}