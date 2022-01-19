using System;
using Spirebyte.Services.Issues.Core.Exceptions.Base;

namespace Spirebyte.Services.Issues.Core.Exceptions;

public class InvalidAuthorIdException : DomainException
{
    public InvalidAuthorIdException(Guid authorId) : base($"Invalid authorId: {authorId}.")
    {
    }

    public override string Code { get; } = "invalid_author_id";
}