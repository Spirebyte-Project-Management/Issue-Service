using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Issues.Application.Issues.Exceptions;

public class EpicNotFoundException : AppException
{
    public EpicNotFoundException(string epicId) : base($"Epic with ID: '{epicId}' was not found.")
    {
        EpicId = epicId;
    }
    public string EpicId { get; }
}