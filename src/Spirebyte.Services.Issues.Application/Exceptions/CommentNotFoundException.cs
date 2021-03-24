using System;
using System.Collections.Generic;
using System.Text;
using Spirebyte.Services.Issues.Application.Exceptions.Base;

namespace Spirebyte.Services.Issues.Application.Exceptions
{
    public class CommentNotFoundException : AppException
    {
        public override string Code { get; } = "comment_not_found";
        public string CommentId { get; }

        public CommentNotFoundException(string commentID) : base($"Comment with ID: '{commentID}' was not found.")
        {
            CommentId = commentID;
        }
    }
}
