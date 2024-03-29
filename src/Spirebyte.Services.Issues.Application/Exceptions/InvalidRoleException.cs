﻿using System;
using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Issues.Application.Exceptions;

public class InvalidRoleException : AppException
{
    public InvalidRoleException(Guid userId, string role, string requiredRole)
        : base($"User account will not be created for the user with id: {userId} " +
               $"due to the invalid role: {role} (required: {requiredRole}).")
    {
    }
}