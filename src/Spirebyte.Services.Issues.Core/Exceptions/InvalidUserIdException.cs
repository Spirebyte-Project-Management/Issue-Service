using System;
using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Issues.Core.Exceptions;

public class InvalidUserIdException : DomainException
{
    public InvalidUserIdException(Guid userId) : base($"Invalid userId: {userId}.")
    {
    }

    public string Code { get; } = "invalid_user_id";
}