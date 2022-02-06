using System;
using Spirebyte.Services.Issues.Application.Issues.DTO;
using Spirebyte.Services.Issues.Core.Entities;

namespace Spirebyte.Services.Issues.Application.Issues.Services.Interfaces;

public interface IIssueRequestStorage
{
    void SetIssue(Guid referenceId, Issue issue);
    IssueDto GetIssue(Guid referenceId);
}