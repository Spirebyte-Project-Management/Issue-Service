using System;
using System.Collections.Generic;
using System.Text;
using Spirebyte.Services.Issues.Core.Exceptions.Base;

namespace Spirebyte.Services.Issues.Core.Exceptions
{
    public class InvalidUserIdException : DomainException
    {
        public override string Code { get; } = "invalid_user_id";

        public InvalidUserIdException(Guid userId) : base($"Invalid userId: {userId}.")
        {
        }
    }
}
