using System.Collections.Generic;
using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Services.Issues.Application.IssueComments.DTO;

namespace Spirebyte.Services.Issues.Application.IssueComments.Queries;

public record GetComments(string? ProjectId, string? IssueId) : IQuery<IEnumerable<CommentDto>>;