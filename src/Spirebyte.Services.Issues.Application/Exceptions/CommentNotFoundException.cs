using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Issues.Application.Exceptions;

public class CommentNotFoundException : AppException
{
    public CommentNotFoundException(string commentId) : base($"Comment with ID: '{commentId}' was not found.")
    {
        CommentId = commentId;
    }
    public string CommentId { get; }
}