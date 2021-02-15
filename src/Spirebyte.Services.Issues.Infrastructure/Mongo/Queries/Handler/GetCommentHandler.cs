
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Spirebyte.Services.Issues.Application;
using Spirebyte.Services.Issues.Application.Clients.Interfaces;
using Spirebyte.Services.Issues.Application.DTO;
using Spirebyte.Services.Issues.Application.Queries;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents.Mappers;

namespace Spirebyte.Services.Issues.Infrastructure.Mongo.Queries.Handler
{
    internal sealed class GetCommentHandler : IQueryHandler<GetComment, CommentDto>
    {
        private readonly IMongoRepository<IssueDocument, string> _issueRepository;
        private readonly IMongoRepository<ProjectDocument, string> _projectRepository;
        private readonly IMongoRepository<CommentDocument, string> _commentRepository;
        private readonly IAppContext _appContext;
        private readonly IProjectsApiHttpClient _projectsApiHttpClient;

        public GetCommentHandler(IMongoRepository<IssueDocument, string> issueRepository, IMongoRepository<ProjectDocument, string> projectRepository, IMongoRepository<CommentDocument, string> commentRepository, IAppContext appContext, IProjectsApiHttpClient projectsApiHttpClient)
        {
            _issueRepository = issueRepository;
            _projectRepository = projectRepository;
            _commentRepository = commentRepository;
            _appContext = appContext;
            _projectsApiHttpClient = projectsApiHttpClient;
        }

        public async Task<CommentDto> HandleAsync(GetComment query)
        {
            var comment = await _commentRepository.GetAsync(query.Id);
            if (comment == null) return null;

            var issue = await _issueRepository.GetAsync(comment.IssueId);
            if (issue == null) return null;

            var project = await _projectRepository.GetAsync(issue.ProjectId);
            if (project == null) return null;

            var identity = _appContext.Identity;
            if (identity.IsAuthenticated)
            {
                var isInProject = await _projectsApiHttpClient.IsProjectUserAsync(project.Id, identity.Id);
                if (!isInProject)
                {
                    return null;
                }
            }

            return comment.AsDto();
        }
    }
}
