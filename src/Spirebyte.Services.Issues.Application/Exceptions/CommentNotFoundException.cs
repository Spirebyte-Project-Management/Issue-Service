using Spirebyte.Services.Issues.Application.Exceptions.Base;

namespace Spirebyte.Services.Issues.Application.Exceptions;

public class CommentNotFoundException : AppException
{
    public CommentNotFoundException(string commentId) : base($"Comment with ID: '{commentId}' was not found.")
    {
        CommentId = commentId;
    }

    public override string Code { get; } = "comment_not_found";
    public string CommentId { get; }
}