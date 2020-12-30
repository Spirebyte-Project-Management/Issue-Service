using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Spirebyte.Services.Issues.Application;
using Spirebyte.Services.Issues.Application.Clients.Interfaces;
using Spirebyte.Services.Issues.Application.DTO;
using Spirebyte.Services.Issues.Application.Queries;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spirebyte.Services.Issues.Infrastructure.Mongo.Queries.Handler
{
    internal sealed class GetIssuesHandler : IQueryHandler<GetIssues, IEnumerable<IssueDto>>
    {
        private readonly IMongoRepository<IssueDocument, string> _issueRepository;
        private readonly IMongoRepository<ProjectDocument, string> _projectRepository;
        private readonly IAppContext _appContext;
        private readonly IProjectsApiHttpClient _projectsApiHttpClient;
        private readonly ISprintsApiHttpClient _sprintsApiHttpClient;

        public GetIssuesHandler(IMongoRepository<IssueDocument, string> issueRepository, IMongoRepository<ProjectDocument, string> projectRepository, IAppContext appContext, IProjectsApiHttpClient projectsApiHttpClient, ISprintsApiHttpClient sprintsApiHttpClient)
        {
            _issueRepository = issueRepository;
            _projectRepository = projectRepository;
            _appContext = appContext;
            _projectsApiHttpClient = projectsApiHttpClient;
            _sprintsApiHttpClient = sprintsApiHttpClient;
        }

        public async Task<IEnumerable<IssueDto>> HandleAsync(GetIssues query)
        {
            var documents = _issueRepository.Collection.AsQueryable();

            if (query.ProjectId == null)
                return Enumerable.Empty<IssueDto>();

            var project = await _projectRepository.GetAsync(query.ProjectId);
            if (project == null)
            {
                return Enumerable.Empty<IssueDto>();
            }

            var identity = _appContext.Identity;
            if (identity.IsAuthenticated)
            {
                var isInProject = await _projectsApiHttpClient.IsProjectUserAsync(query.ProjectId, identity.Id);
                if (!isInProject)
                {
                    return Enumerable.Empty<IssueDto>();
                }
            }

            string[] sprintIssueIds = null;
            if (query.HasSprint != null)
            {
                sprintIssueIds = await _sprintsApiHttpClient.IssuesWithoutSprintForProject(query.ProjectId);
                if (sprintIssueIds == null) return Enumerable.Empty<IssueDto>();
            }

            var filter = new Func<IssueDocument, bool>(p =>
                p.ProjectId == project.Id
                && (query.Type == null || p.Type == query.Type.Value)
                && (query.IssueIds == null || query.IssueIds.Contains(p.Id))
                && (query.HasSprint == null || (query.HasSprint.Value ? !sprintIssueIds!.Contains(p.Id) : sprintIssueIds!.Contains(p.Id))));

            var issues = documents.Where(filter).ToList();

            return issues.Select(p => p.AsDto());
        }
    }
}
