﻿using Convey.CQRS.Events;
using System;

namespace Spirebyte.Services.Issues.Application.Events
{
    [Contract]
    public class IssueCreated : IEvent
    {
        public Guid IssueId { get; }
        public string IssueKey { get; }
        public Guid ProjectId { get; }

        public IssueCreated(Guid issueId, string issueKey, Guid projectId)
        {
            IssueId = issueId;
            IssueKey = issueKey;
            ProjectId = projectId;
        }
    }
}
