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
using Spirebyte.Services.Issues.Core.Enums;

namespace Spirebyte.Services.Issues.Infrastructure.Mongo.Queries.Handler
{
    internal sealed class GetEpicsHandler : IQueryHandler<GetEpics, IEnumerable<IssueDto>>
    {
        private readonly IMongoRepository<IssueDocument, Guid> _issueRepository;
        private readonly IMongoRepository<ProjectDocument, Guid> _projectRepository;
        private readonly IAppContext _appContext;
        private readonly IProjectsApiHttpClient _projectsApiHttpClient;

        public GetEpicsHandler(IMongoRepository<IssueDocument, Guid> issueRepository, IMongoRepository<ProjectDocument, Guid> projectRepository, IAppContext appContext, IProjectsApiHttpClient projectsApiHttpClient)
        {
            _issueRepository = issueRepository;
            _projectRepository = projectRepository;
            _appContext = appContext;
            _projectsApiHttpClient = projectsApiHttpClient;
        }

        public async Task<IEnumerable<IssueDto>> HandleAsync(GetEpics query)
        {
            var documents = _issueRepository.Collection.AsQueryable();

            var project = await _projectRepository.GetAsync(c => c.Key == query.ProjectKey);
            if (project == null)
            {
                return Enumerable.Empty<IssueDto>();
            }

            var identity = _appContext.Identity;
            if (identity.IsAuthenticated)
            {
                var isInProject = await _projectsApiHttpClient.IsProjectUserAsync(query.ProjectKey, identity.Id);
                if (!isInProject)
                {
                    return Enumerable.Empty<IssueDto>();
                }
            }

            var issues = await documents.Where(p => p.ProjectId == project.Id && p.Type == IssueType.Epic).ToListAsync();

            return issues.Select(p => p.AsDto());
        }
    }
}
