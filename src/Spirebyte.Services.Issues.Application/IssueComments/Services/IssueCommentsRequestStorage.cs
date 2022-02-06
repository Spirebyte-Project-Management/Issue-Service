using System;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using Spirebyte.Services.Issues.Application.Contexts;
using Spirebyte.Services.Issues.Application.IssueComments.DTO;
using Spirebyte.Services.Issues.Application.IssueComments.Services.Interfaces;
using Spirebyte.Services.Issues.Core.Entities;

namespace Spirebyte.Services.Issues.Application.IssueComments.Services;

public class IssueCommentsRequestStorage : IIssueCommentsRequestStorage
{
    private readonly IAppContext _appContext;
    private readonly IMemoryCache _cache;

    public IssueCommentsRequestStorage(IMemoryCache cache, IAppContext appContext)
    {
        _cache = cache;
        _appContext = appContext;
    }

    public void SetComment(Guid referenceId, Comment comment)
    {
        var commentDto = new CommentDto
        {
            Id = comment.Id,
            IssueId = comment.IssueId,
            ProjectId = comment.ProjectId,
            AuthorId = comment.AuthorId,
            Body = comment.Body,
            Reactions = comment.Reactions ?? Enumerable.Empty<Reaction>(),
            CreatedAt = comment.CreatedAt,
            UpdatedAt = comment.UpdatedAt,
            CanEdit = comment.AuthorId == _appContext.Identity.Id,
            CanDelete = comment.AuthorId == _appContext.Identity.Id
        };

        _cache.Set(GetKey(referenceId), commentDto, TimeSpan.FromSeconds(5));
    }

    public CommentDto GetComment(Guid referenceId)
    {
        return _cache.Get<CommentDto>(GetKey(referenceId));
    }

    private static string GetKey(Guid commandId)
    {
        return $"comment:{commandId:N}";
    }
}