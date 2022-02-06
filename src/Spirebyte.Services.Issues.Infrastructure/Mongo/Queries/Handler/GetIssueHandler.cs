using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Spirebyte.Services.Issues.Application.Clients.Interfaces;
using Spirebyte.Services.Issues.Application.Contexts;
using Spirebyte.Services.Issues.Application.Issues.DTO;
using Spirebyte.Services.Issues.Application.Issues.Queries;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents.Mappers;

namespace Spirebyte.Services.Issues.Infrastructure.Mongo.Queries.Handler;

internal sealed class GetIssueHandler : IQueryHandler<GetIssue, IssueDto>
{
    private readonly IAppContext _appContext;
    private readonly IMongoRepository<IssueDocument, string> _issueRepository;
    private readonly IMongoRepository<ProjectDocument, string> _projectRepository;
    private readonly IProjectsApiHttpClient _projectsApiHttpClient;

    public GetIssueHandler(IMongoRepository<IssueDocument, string> issueRepository,
        IMongoRepository<ProjectDocument, string> projectRepository, IAppContext appContext,
        IProjectsApiHttpClient projectsApiHttpClient)
    {
        _issueRepository = issueRepository;
        _projectRepository = projectRepository;
        _appContext = appContext;
        _projectsApiHttpClient = projectsApiHttpClient;
    }

    public async Task<IssueDto> HandleAsync(GetIssue query, CancellationToken cancellationToken = default)
    {
        var issue = await _issueRepository.GetAsync(p => p.Id == query.Id);
        if (issue == null) return null;

        var project = await _projectRepository.GetAsync(issue.ProjectId);
        if (project == null) return null;

        return issue.AsDto();
    }
}