using Convey.CQRS.Events;

namespace Spirebyte.Services.Issues.Application.Issues.Events;

[Contract]
public record IssueCreated(string IssueId, string ProjectId, int StoryPoints) : IEvent;