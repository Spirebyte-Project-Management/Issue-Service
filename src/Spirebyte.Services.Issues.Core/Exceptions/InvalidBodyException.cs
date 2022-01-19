using Spirebyte.Services.Issues.Core.Exceptions.Base;

namespace Spirebyte.Services.Issues.Core.Exceptions;

public class InvalidBodyException : DomainException
{
    public InvalidBodyException(string body) : base($"Invalid body: {body}.")
    {
    }

    public override string Code { get; } = "invalid_body";
}