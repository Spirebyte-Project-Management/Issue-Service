using Convey.Types;
using Spirebyte.Services.Issues.Core.Enums;
using System;
using System.Collections.Generic;

namespace Spirebyte.Services.Issues.Infrastructure.Mongo.Documents
{
    internal sealed class IssueDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public IssueType Type { get; set; }
        public IssueStatus Status { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int StoryPoints { get; set; }

        public Guid ProjectId { get; set; }
        public Guid EpicId { get; set; }
        public IEnumerable<Guid> Assignees { get; set; }
        public IEnumerable<Guid> LinkedIssues { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
