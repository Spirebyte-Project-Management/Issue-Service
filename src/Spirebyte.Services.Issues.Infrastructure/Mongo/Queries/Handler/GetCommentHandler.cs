using System.Threading;
using System.Threading.Tasks;
using Spirebyte.Framework.Contexts;
using Spirebyte.Framework.DAL.MongoDb.Interfaces;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Services.Issues.Application.Clients.Interfaces;
using Spirebyte.Services.Issues.Application.IssueComments.DTO;
using Spirebyte.Services.Issues.Application.IssueComments.Queries;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents.Mappers;

namespace Spirebyte.Services.Issues.Infrastructure.Mongo.Queries.Handler;

internal sealed class GetCommentHandler : IQueryHandler<GetComment, CommentDto?>
{
    private readonly IMongoRepository<CommentDocument, string> _commentRepository;
    private readonly IContextAccessor _contextAccessor;
    private readonly IMongoRepository<IssueDocument, string> _issueRepository;
    private readonly IMongoRepository<ProjectDocument, string> _projectRepository;
    private readonly IProjectsApiHttpClient _projectsApiHttpClient;

    public GetCommentHandler(IMongoRepository<IssueDocument, string> issueRepository,
        IMongoRepository<ProjectDocument, string> projectRepository,
        IMongoRepository<CommentDocument, string> commentRepository, IContextAccessor contextAccessor,
        IProjectsApiHttpClient projectsApiHttpClient)
    {
        _issueRepository = issueRepository;
        _projectRepository = projectRepository;
        _commentRepository = commentRepository;
        _contextAccessor = contextAccessor;
        _projectsApiHttpClient = projectsApiHttpClient;
    }

    public async Task<CommentDto?> HandleAsync(GetComment query, CancellationToken cancellationToken = default)
    {
        var comment = await _commentRepository.GetAsync(query.Id);
        if (comment == null) return null;

        var issue = await _issueRepository.GetAsync(comment.IssueId);
        if (issue == null) return null;

        var project = await _projectRepository.GetAsync(issue.ProjectId);
        if (project == null) return null;

        return comment.AsDto(_contextAccessor.Context.GetUserId());
    }
}