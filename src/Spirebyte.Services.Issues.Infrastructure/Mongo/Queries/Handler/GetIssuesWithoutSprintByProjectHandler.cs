using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Spirebyte.Services.Issues.Application;
using Spirebyte.Services.Issues.Application.Clients.Interfaces;
using Spirebyte.Services.Issues.Application.DTO;
using Spirebyte.Services.Issues.Application.Queries;
using Spirebyte.Services.Issues.Core.Repositories;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spirebyte.Services.Issues.Infrastructure.Mongo.Queries.Handler
{
    internal sealed class GetIssuesWithoutSprintByProjectHandler : IQueryHandler<GetIssuesWithoutSprintByProject, IEnumerable<IssueDto>>
    {
        private readonly IMongoRepository<IssueDocument, Guid> _issueRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IAppContext _appContext;
        private readonly IProjectsApiHttpClient _projectsApiHttpClient;
        private readonly ISprintsApiHttpClient _sprintsApiHttpClient;

        public GetIssuesWithoutSprintByProjectHandler(IMongoRepository<IssueDocument, Guid> issueRepository, IProjectRepository projectRepository, IAppContext appContext, IProjectsApiHttpClient projectsApiHttpClient, ISprintsApiHttpClient sprintsApiHttpClient)
        {
            _issueRepository = issueRepository;
            _projectRepository = projectRepository;
            _appContext = appContext;
            _projectsApiHttpClient = projectsApiHttpClient;
            _sprintsApiHttpClient = sprintsApiHttpClient;
        }

        public async Task<IEnumerable<IssueDto>> HandleAsync(GetIssuesWithoutSprintByProject query)
        {
            if (!await _projectRepository.ExistsAsync(query.ProjectKey)) return null;

            var identity = _appContext.Identity;
            if (identity.IsAuthenticated)
            {
                var isInProject = await _projectsApiHttpClient.IsProjectUserAsync(query.ProjectKey, identity.Id);
                if (!isInProject)
                {
                    return Enumerable.Empty<IssueDto>();
                }
            }

            var issueIds = await _sprintsApiHttpClient.IssuesWithoutSprintForProject(query.ProjectKey);
            if (issueIds == null) return Enumerable.Empty<IssueDto>();

            var documents = _issueRepository.Collection.AsQueryable();

            var issues = await documents.Where(u => issueIds.Contains(u.Id)).OrderBy(i => i.Key).ToListAsync();

            return issues.Select(i => i.AsDto());
        }
    }
}
