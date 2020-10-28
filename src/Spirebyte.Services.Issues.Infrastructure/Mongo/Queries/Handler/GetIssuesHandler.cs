using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Spirebyte.Services.Issues.Application;
using Spirebyte.Services.Issues.Application.DTO;
using Spirebyte.Services.Issues.Application.Queries;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Spirebyte.Services.Issues.Application.Clients.Interfaces;

namespace Spirebyte.Services.Issues.Infrastructure.Mongo.Queries.Handler
{
    internal sealed class GetIssuesHandler : IQueryHandler<GetIssues, IEnumerable<IssueDto>>
    {
        private readonly IMongoRepository<IssueDocument, Guid> _issueRepository;
        private readonly IAppContext _appContext;
        private readonly IProjectsApiHttpClient _projectsApiHttpClient;

        public GetIssuesHandler(IMongoRepository<IssueDocument, Guid> issueRepository, IAppContext appContext, IProjectsApiHttpClient projectsApiHttpClient)
        {
            _issueRepository = issueRepository;
            _appContext = appContext;
            _projectsApiHttpClient = projectsApiHttpClient;
        }

        public async Task<IEnumerable<IssueDto>> HandleAsync(GetIssues query)
        {
            var documents = _issueRepository.Collection.AsQueryable();

            var identity = _appContext.Identity;
            if (identity.IsAuthenticated)
            {
                var isInProject = await _projectsApiHttpClient.IsProjectUserAsync(query.ProjectId, identity.Id);
                if (!isInProject)
                {
                    return Enumerable.Empty<IssueDto>();
                }
            }

            var issues = await documents.Where(p => p.ProjectId == query.ProjectId).ToListAsync();

            return issues.Select(p => p.AsDto());
        }
    }
}
