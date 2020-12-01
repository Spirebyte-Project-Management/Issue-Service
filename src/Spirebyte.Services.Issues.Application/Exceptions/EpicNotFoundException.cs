using Spirebyte.Services.Issues.Application.Exceptions.Base;
using System;

namespace Spirebyte.Services.Issues.Application.Exceptions
{
    public class EpicNotFoundException : AppException
    {
        public override string Code { get; } = "epic_not_found";
        public Guid EpicId { get; }

        public EpicNotFoundException(Guid epicId) : base($"Epic with ID: '{epicId}' was not found.")
        {
            EpicId = epicId;
        }
    }
}
