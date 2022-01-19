using System;
using Spirebyte.Services.Issues.Core.Exceptions.Base;

namespace Spirebyte.Services.Issues.Core.Exceptions;

public class InvalidUserIdException : DomainException
{
    public InvalidUserIdException(Guid userId) : base($"Invalid userId: {userId}.")
    {
    }

    public override string Code { get; } = "invalid_user_id";
}