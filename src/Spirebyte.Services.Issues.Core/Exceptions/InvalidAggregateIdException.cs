﻿using Spirebyte.Services.Issues.Core.Exceptions.Base;

namespace Spirebyte.Services.Issues.Core.Exceptions
{
    public class InvalidAggregateIdException : DomainException
    {
        public override string Code { get; } = "invalid_aggregate_id";

        public InvalidAggregateIdException() : base($"Invalid aggregate id.")
        {
        }
    }
}
