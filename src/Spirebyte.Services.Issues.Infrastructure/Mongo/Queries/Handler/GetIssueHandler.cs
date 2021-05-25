using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Spirebyte.Services.Issues.Application;
using Spirebyte.Services.Issues.Application.Clients.Interfaces;
using Spirebyte.Services.Issues.Application.DTO;
using Spirebyte.Services.Issues.Application.Queries;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents.Mappers;
using System.Threading.Tasks;

namespace Spirebyte.Services.Issues.Infrastructure.Mongo.Queries.Handler
{
    internal sealed class GetIssueHandler : IQueryHandler<GetIssue, IssueDto>
    {
        private readonly IMongoRepository<IssueDocument, string> _issueRepository;
        private readonly IMongoRepository<ProjectDocument, string> _projectRepository;
        private readonly IAppContext _appContext;
        private readonly IProjectsApiHttpClient _projectsApiHttpClient;

        public GetIssueHandler(IMongoRepository<IssueDocument, string> issueRepository, IMongoRepository<ProjectDocument, string> projectRepository, IAppContext appContext, IProjectsApiHttpClient projectsApiHttpClient)
        {
            _issueRepository = issueRepository;
            _projectRepository = projectRepository;
            _appContext = appContext;
            _projectsApiHttpClient = projectsApiHttpClient;
        }

        public async Task<IssueDto> HandleAsync(GetIssue query)
        {
            var issue = await _issueRepository.GetAsync(p => p.Id == query.Id);
            if (issue == null) return null;

            var project = await _projectRepository.GetAsync(issue.ProjectId);
            if (project == null) return null;

            return issue.AsDto();
        }
    }
}
