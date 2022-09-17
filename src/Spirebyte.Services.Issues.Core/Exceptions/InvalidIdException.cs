using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Issues.Core.Exceptions;

public class InvalidIdException : DomainException
{
    public InvalidIdException(string key) : base($"Invalid key: {key}.")
    {
    }

    public string Code { get; } = "invalid_key";
}