using System;
using System.Collections.Generic;
using Spirebyte.Framework.Shared.Types;
using Spirebyte.Services.Issues.Core.Enums;

namespace Spirebyte.Services.Issues.Infrastructure.Mongo.Documents;

public sealed class IssueDocument : IIdentifiable<string>
{
    public IssueType Type { get; set; }
    public IssueStatus Status { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int StoryPoints { get; set; }

    public string ProjectId { get; set; }
    public string EpicId { get; set; }
    public string SprintId { get; set; }
    public IEnumerable<Guid> Assignees { get; set; }
    public IEnumerable<Guid> LinkedIssues { get; set; }

    public DateTime CreatedAt { get; set; }
    public string Id { get; set; }
}