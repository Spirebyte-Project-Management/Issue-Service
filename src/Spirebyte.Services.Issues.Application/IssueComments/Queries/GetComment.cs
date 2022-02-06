using Convey.CQRS.Queries;
using Spirebyte.Services.Issues.Application.IssueComments.DTO;

namespace Spirebyte.Services.Issues.Application.IssueComments.Queries;

public record GetComment(string Id) : IQuery<CommentDto>;