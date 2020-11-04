

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

namespace Spirebyte.Services.Issues.Infrastructure.Mongo.Queries.Handler
{
    internal sealed class GetIssueHandler : IQueryHandler<GetIssue, IssueDto>
    {
        private readonly IMongoRepository<IssueDocument, Guid> _issueRepository;
        private readonly IMongoRepository<ProjectDocument, Guid> _projectRepository;
        private readonly IAppContext _appContext;
        private readonly IProjectsApiHttpClient _projectsApiHttpClient;

        public GetIssueHandler(IMongoRepository<IssueDocument, Guid> issueRepository, IMongoRepository<ProjectDocument, Guid> projectRepository, IAppContext appContext, IProjectsApiHttpClient projectsApiHttpClient)
        {
            _issueRepository = issueRepository;
            _projectRepository = projectRepository;
            _appContext = appContext;
            _projectsApiHttpClient = projectsApiHttpClient;
        }

        public async Task<IssueDto> HandleAsync(GetIssue query)
        {
            var issue = await _issueRepository.GetAsync(p => p.Key == query.IssueKey);
            if (issue == null) return null;

            var project = await _projectRepository.GetAsync(issue.ProjectId);
            if (project == null) return null;


            var identity = _appContext.Identity;
            if (identity.IsAuthenticated)
            {
                var isInProject = await _projectsApiHttpClient.IsProjectUserAsync(project.Key, identity.Id);
                if (!isInProject)
                {
                    return null;
                }
            }

            return issue.AsDto();
        }
    }
}
