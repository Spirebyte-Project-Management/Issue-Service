using System;
using System.Collections.Generic;
using System.Text;
using Spirebyte.Services.Issues.Core.Exceptions.Base;

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
