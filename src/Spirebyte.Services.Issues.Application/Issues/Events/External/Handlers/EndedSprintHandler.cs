using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using Spirebyte.Services.Issues.Core.Repositories;

namespace Spirebyte.Services.Issues.Application.Issues.Events.External.Handlers;

public class EndedSprintHandler : IEventHandler<EndedSprint>
{
    private readonly IIssueRepository _issueRepository;


    public EndedSprintHandler(IIssueRepository issueRepository)
    {
        _issueRepository = issueRepository;
    }

    public async Task HandleAsync(EndedSprint @event, CancellationToken cancellationToken = default)
    {
        var issues = await _issueRepository.GetIssuesWithSprint(@event.SprintId);

        foreach (var issue in issues)
        {
            issue.RemoveFromSprint();
            await _issueRepository.UpdateAsync(issue);
        }
    }
}