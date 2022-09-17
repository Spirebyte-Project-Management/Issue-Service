using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Issues.Core.Exceptions;

public class InvalidBodyException : DomainException
{
    public InvalidBodyException(string body) : base($"Invalid body: {body}.")
    {
    }

    public string Code { get; } = "invalid_body";
}