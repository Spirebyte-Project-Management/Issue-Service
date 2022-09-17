using System.Collections.Generic;
using System.Text.Json.Serialization;
using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Services.Issues.Application.Issues.DTO;
using Spirebyte.Services.Issues.Core.Enums;

namespace Spirebyte.Services.Issues.Application.Issues.Queries;

public class GetIssues : IQuery<IEnumerable<IssueDto>>
{
    [JsonConstructor]
    public GetIssues()
    {
    }

    public GetIssues(string? projectId = null, IssueType? issueType = null, bool? hasSprint = null,
        string[]? issueIds = null)
    {
        ProjectId = projectId;
        Type = issueType;
        HasSprint = hasSprint;
        IssueIds = issueIds;
    }

    public string? ProjectId { get; set; }
    public IssueType? Type { get; set; }
    public bool? HasSprint { get; set; }
    public string[]? IssueIds { get; set; }
}