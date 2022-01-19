using System.Threading.Tasks;
using Convey.CQRS.Events;
using Spirebyte.Services.Issues.Core.Repositories;

namespace Spirebyte.Services.Issues.Application.Events.External.Handlers;

public class EndedSprintHandler : IEventHandler<EndedSprint>
{
    private readonly IIssueRepository _issueRepository;


    public EndedSprintHandler(IIssueRepository issueRepository)
    {
        _issueRepository = issueRepository;
    }

    public async Task HandleAsync(EndedSprint @event)
    {
        var issues = await _issueRepository.GetIssuesWithSprint(@event.SprintId);

        foreach (var issue in issues)
        {
            issue.RemoveFromSprint();
            await _issueRepository.UpdateAsync(issue);
        }
    }
}