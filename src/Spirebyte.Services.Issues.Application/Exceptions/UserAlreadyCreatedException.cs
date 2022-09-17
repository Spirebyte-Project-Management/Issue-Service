using System;
using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Issues.Application.Exceptions;

public class UserAlreadyCreatedException : AppException
{
    public UserAlreadyCreatedException(Guid userId)
        : base($"User with id: {userId} was already created.")
    {
        UserId = userId;
    }
    public Guid UserId { get; }
}