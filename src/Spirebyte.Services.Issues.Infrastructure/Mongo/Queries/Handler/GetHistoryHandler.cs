using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Spirebyte.Services.Issues.Application.DTO;
using Spirebyte.Services.Issues.Application.Queries;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Spirebyte.Services.Issues.Application;
using Spirebyte.Services.Issues.Application.Clients.Interfaces;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents.Mappers;

namespace Spirebyte.Services.Issues.Infrastructure.Mongo.Queries.Handler
{
    internal sealed class GetHistoryHandler : IQueryHandler<GetHistory, IEnumerable<HistoryDto>>
    {
        private readonly IMongoRepository<HistoryDocument, Guid> _historyRepository;
        private readonly IMongoRepository<IssueDocument, string> _issueRepository;
        private readonly IAppContext _appContext;
        private readonly IProjectsApiHttpClient _projectsApiHttpClient;

        public GetHistoryHandler(IMongoRepository<HistoryDocument, Guid> historyRepository, IMongoRepository<IssueDocument, string> issueRepository, IAppContext appContext, IProjectsApiHttpClient projectsApiHttpClient)
        {
            _historyRepository = historyRepository;
            _issueRepository = issueRepository;
            _appContext = appContext;
            _projectsApiHttpClient = projectsApiHttpClient;
        }
        public async Task<IEnumerable<HistoryDto>> HandleAsync(GetHistory query)
        {
            var documents = _historyRepository.Collection.AsQueryable();

            var issue = await _issueRepository.GetAsync(query.IssueId);
            if (query.IssueId != null && issue == null) return Enumerable.Empty<HistoryDto>();


            if (!await IsUserInProject(issue.ProjectId))
            {
                return Enumerable.Empty<HistoryDto>();
            }

            var filter = new Func<HistoryDocument, bool>(p =>
                query.IssueId == null || p.IssueId == query.IssueId);

            var history = documents.Where(filter).ToList();

            return history.Select(p => p.AsDto());
        }

        private async Task<bool> IsUserInProject(string projectId)
        {
            var identity = _appContext.Identity;
            if (identity.IsAuthenticated)
            {
                return await _projectsApiHttpClient.IsProjectUserAsync(projectId, identity.Id);
            }

            return false;
        }
    }
}
