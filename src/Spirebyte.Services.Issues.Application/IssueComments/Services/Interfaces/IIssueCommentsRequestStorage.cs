using System;
using Spirebyte.Services.Issues.Application.IssueComments.DTO;
using Spirebyte.Services.Issues.Core.Entities;

namespace Spirebyte.Services.Issues.Application.IssueComments.Services.Interfaces;

public interface IIssueCommentsRequestStorage
{
    void SetComment(Guid referenceId, Comment comment);
    CommentDto GetComment(Guid referenceId);
}