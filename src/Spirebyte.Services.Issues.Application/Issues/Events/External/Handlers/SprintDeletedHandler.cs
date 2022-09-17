using System.Threading;
using System.Threading.Tasks;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Services.Issues.Core.Repositories;

namespace Spirebyte.Services.Issues.Application.Issues.Events.External.Handlers;

public class SprintDeletedHandler : IEventHandler<SprintDeleted>
{
    private readonly IIssueRepository _issueRepository;


    public SprintDeletedHandler(IIssueRepository issueRepository)
    {
        _issueRepository = issueRepository;
    }

    public async Task HandleAsync(SprintDeleted @event, CancellationToken cancellationToken = default)
    {
        var issues = await _issueRepository.GetIssuesWithSprint(@event.SprintId);

        foreach (var issue in issues)
        {
            issue.RemoveFromSprint();
            await _issueRepository.UpdateAsync(issue);
        }
    }
}