using System;
using System.Collections.Generic;
using System.Text;
using Spirebyte.Services.Issues.Core.Exceptions.Base;

namespace Spirebyte.Services.Issues.Core.Exceptions
{
    public class InvalidIssueIdException : DomainException
    {
        public override string Code { get; } = "invalid_issue_id";

        public InvalidIssueIdException(string issueId) : base($"Invalid issueId: {issueId}.")
        {
        }
    }
}
