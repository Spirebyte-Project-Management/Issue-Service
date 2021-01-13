using System;
using System.Collections.Generic;
using System.Text;
using Convey.CQRS.Events;

namespace Spirebyte.Services.Issues.Application.Events
{
    [Contract]
    public class IssueDeleted : IEvent
    {
        public string IssueId { get; }
        public string ProjectId { get; set; }

        public IssueDeleted(string issueId, string projectId)
        {
            IssueId = issueId;
            ProjectId = projectId;
        }
    }
}
