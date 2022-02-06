using Convey.CQRS.Events;

namespace Spirebyte.Services.Issues.Application.Issues.Events;

[Contract]
public record IssueDeleted(string IssueId, string ProjectId) : IEvent;