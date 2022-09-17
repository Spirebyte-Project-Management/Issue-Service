using System;
using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Issues.Core.Exceptions;

public class InvalidAuthorIdException : DomainException
{
    public InvalidAuthorIdException(Guid authorId) : base($"Invalid authorId: {authorId}.")
    {
    }

    public string Code { get; } = "invalid_author_id";
}