using Convey.CQRS.Events;
using Spirebyte.Services.Issues.Core.Enums;

namespace Spirebyte.Services.Issues.Application.Issues.Events;

[Contract]
public record IssueUpdated(string IssueId, int StoryPoints, IssueStatus Status) : IEvent;