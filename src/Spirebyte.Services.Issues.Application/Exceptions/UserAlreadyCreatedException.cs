using System;
using Spirebyte.Services.Issues.Application.Exceptions.Base;

namespace Spirebyte.Services.Issues.Application.Exceptions;

public class UserAlreadyCreatedException : AppException
{
    public UserAlreadyCreatedException(Guid userId)
        : base($"User with id: {userId} was already created.")
    {
        UserId = userId;
    }

    public override string Code { get; } = "user_already_created";
    public Guid UserId { get; }
}