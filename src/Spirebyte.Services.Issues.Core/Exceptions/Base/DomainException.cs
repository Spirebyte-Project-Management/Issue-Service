using System;

namespace Spirebyte.Services.Issues.Core.Exceptions.Base;

public abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message)
    {
    }

    public virtual string Code { get; }
}