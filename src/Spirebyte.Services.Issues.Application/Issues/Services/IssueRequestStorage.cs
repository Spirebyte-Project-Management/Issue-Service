using System;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using Spirebyte.Services.Issues.Application.Issues.DTO;
using Spirebyte.Services.Issues.Application.Issues.Services.Interfaces;
using Spirebyte.Services.Issues.Core.Entities;

namespace Spirebyte.Services.Issues.Application.Issues.Services;

public class IssueRequestStorage : IIssueRequestStorage
{
    private readonly IMemoryCache _cache;

    public IssueRequestStorage(IMemoryCache cache)
    {
        _cache = cache;
    }

    public void SetIssue(Guid referenceId, Issue issue)
    {
        var issueDto = new IssueDto
        {
            Id = issue.Id,
            Type = issue.Type,
            Status = issue.Status,
            Title = issue.Title,
            Description = issue.Description,
            StoryPoints = issue.StoryPoints,
            ProjectId = issue.ProjectId,
            EpicId = issue.EpicId,
            SprintId = issue.SprintId,
            Assignees = issue.Assignees ?? Enumerable.Empty<Guid>(),
            LinkedIssues = issue.LinkedIssues ?? Enumerable.Empty<Guid>(),
            CreatedAt = issue.CreatedAt
        };

        _cache.Set(GetKey(referenceId), issueDto, TimeSpan.FromSeconds(5));
    }

    public IssueDto GetIssue(Guid referenceId)
    {
        return _cache.Get<IssueDto>(GetKey(referenceId));
    }

    private static string GetKey(Guid commandId)
    {
        return $"issue:{commandId:N}";
    }
}