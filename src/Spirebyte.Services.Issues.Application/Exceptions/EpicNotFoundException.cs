using Spirebyte.Services.Issues.Application.Exceptions.Base;

namespace Spirebyte.Services.Issues.Application.Exceptions
{
    public class EpicNotFoundException : AppException
    {
        public override string Code { get; } = "epic_not_found";
        public string EpicId { get; }

        public EpicNotFoundException(string epicId) : base($"Epic with ID: '{epicId}' was not found.")
        {
            EpicId = epicId;
        }
    }
}
