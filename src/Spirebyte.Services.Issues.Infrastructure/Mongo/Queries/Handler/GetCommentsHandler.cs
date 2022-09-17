using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using Spirebyte.Framework.Contexts;
using Spirebyte.Framework.DAL.MongoDb.Interfaces;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Services.Issues.Application.Clients.Interfaces;
using Spirebyte.Services.Issues.Application.IssueComments.DTO;
using Spirebyte.Services.Issues.Application.IssueComments.Queries;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents.Mappers;

namespace Spirebyte.Services.Issues.Infrastructure.Mongo.Queries.Handler;

internal sealed class GetCommentsHandler : IQueryHandler<GetComments, IEnumerable<CommentDto>>
{
    private readonly IContextAccessor _contextAccessor;
    private readonly IMongoRepository<CommentDocument, string> _commentRepository;
    private readonly IMongoRepository<IssueDocument, string> _issueRepository;
    private readonly IMongoRepository<ProjectDocument, string> _projectRepository;
    private readonly IProjectsApiHttpClient _projectsApiHttpClient;

    public GetCommentsHandler(IMongoRepository<IssueDocument, string> issueRepository,
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

    public async Task<IEnumerable<CommentDto>> HandleAsync(GetComments query,
        CancellationToken cancellationToken = default)
    {
        var documents = _commentRepository.Collection.AsQueryable();

        if (query.ProjectId == null && query.IssueId == null)
            return Enumerable.Empty<CommentDto>();

        var issue = await _issueRepository.GetAsync(query.IssueId);
        if (query.IssueId != null && issue == null) return Enumerable.Empty<CommentDto>();

        var project = await _projectRepository.GetAsync(query.ProjectId);
        if (query.ProjectId != null && project == null) return Enumerable.Empty<CommentDto>();

        var filter = new Func<CommentDocument, bool>(p =>
            (query.ProjectId == null || p.ProjectId == query.ProjectId)
            && (query.IssueId == null || p.IssueId == query.IssueId));

        var comments = documents.Where(filter).ToList();

        return comments.Select(p => p.AsDto(_contextAccessor.Context.GetUserId()));
    }
}