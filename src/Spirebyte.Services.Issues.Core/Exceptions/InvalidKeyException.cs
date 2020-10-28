using Spirebyte.Services.Issues.Core.Exceptions.Base;

namespace Spirebyte.Services.Issues.Core.Exceptions
{
    public class InvalidKeyException : DomainException
    {
        public override string Code { get; } = "invalid_key";

        public InvalidKeyException(string key) : base($"Invalid key: {key}.")
        {
        }
    }
}
