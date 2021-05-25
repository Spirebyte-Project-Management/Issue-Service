using Spirebyte.Services.Issues.Core.Exceptions.Base;
using System;

namespace Spirebyte.Services.Issues.Core.Exceptions
{
    public class InvalidAuthorIdException : DomainException
    {
        public override string Code { get; } = "invalid_author_id";

        public InvalidAuthorIdException(Guid authorId) : base($"Invalid authorId: {authorId}.")
        {
        }
    }
}
