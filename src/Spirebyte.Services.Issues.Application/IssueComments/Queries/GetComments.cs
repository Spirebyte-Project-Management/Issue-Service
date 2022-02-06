using System.Collections.Generic;
using Convey.CQRS.Queries;
using Spirebyte.Services.Issues.Application.IssueComments.DTO;

namespace Spirebyte.Services.Issues.Application.IssueComments.Queries;

public record GetComments(string? ProjectId, string? IssueId) : IQuery<IEnumerable<CommentDto>>;