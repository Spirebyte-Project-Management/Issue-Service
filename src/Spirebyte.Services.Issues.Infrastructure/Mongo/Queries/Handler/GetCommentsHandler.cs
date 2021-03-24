using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Spirebyte.Services.Issues.Application;
using Spirebyte.Services.Issues.Application.Clients.Interfaces;
using Spirebyte.Services.Issues.Application.DTO;
using Spirebyte.Services.Issues.Application.Queries;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents.Mappers;

namespace Spirebyte.Services.Issues.Infrastructure.Mongo.Queries.Handler
{
    internal sealed class GetCommentsHandler : IQueryHandler<GetComments, IEnumerable<CommentDto>>
    {
        private readonly IMongoRepository<IssueDocument, string> _issueRepository;
        private readonly IMongoRepository<ProjectDocument, string> _projectRepository;
        private readonly IMongoRepository<CommentDocument, string> _commentRepository;
        private readonly IAppContext _appContext;
        private readonly IProjectsApiHttpClient _projectsApiHttpClient;

        public GetCommentsHandler(IMongoRepository<IssueDocument, string> issueRepository, IMongoRepository<ProjectDocument, string> projectRepository, IMongoRepository<CommentDocument, string> commentRepository, IAppContext appContext, IProjectsApiHttpClient projectsApiHttpClient)
        {
            _issueRepository = issueRepository;
            _projectRepository = projectRepository;
            _commentRepository = commentRepository;
            _appContext = appContext;
            _projectsApiHttpClient = projectsApiHttpClient;
        }

        public async Task<IEnumerable<CommentDto>> HandleAsync(GetComments query)
        {
            var documents = _commentRepository.Collection.AsQueryable();

            if (query.ProjectId == null && query.IssueId == null)
                return Enumerable.Empty<CommentDto>();

            var issue = await _issueRepository.GetAsync(query.IssueId);
            if (query.IssueId != null && issue == null) return Enumerable.Empty<CommentDto>();

            var project = await _projectRepository.GetAsync(query.ProjectId);
            if (query.ProjectId != null && project == null) return Enumerable.Empty<CommentDto>();

            if (!await IsUserInProject(project == null ? issue.ProjectId : project.Id))
            {
                return Enumerable.Empty<CommentDto>();
            }

            var filter = new Func<CommentDocument, bool>(p =>
                (query.ProjectId == null || p.ProjectId == query.ProjectId)
                && (query.IssueId == null || p.IssueId == query.IssueId));

            var comments = documents.Where(filter).ToList();

            return comments.Select(p => p.AsDto(_appContext.Identity));
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
