﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using Spirebyte.Framework.DAL.MongoDb.Interfaces;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Services.Issues.Application.Clients.Interfaces;
using Spirebyte.Services.Issues.Application.Issues.DTO;
using Spirebyte.Services.Issues.Application.Issues.Queries;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents.Mappers;

namespace Spirebyte.Services.Issues.Infrastructure.Mongo.Queries.Handler;

internal sealed class GetIssuesHandler : IQueryHandler<GetIssues, IEnumerable<IssueDto>>
{
    private readonly IMongoRepository<IssueDocument, string> _issueRepository;
    private readonly IMongoRepository<ProjectDocument, string> _projectRepository;
    private readonly ISprintsApiHttpClient _sprintsApiHttpClient;

    public GetIssuesHandler(IMongoRepository<IssueDocument, string> issueRepository,
        IMongoRepository<ProjectDocument, string> projectRepository, ISprintsApiHttpClient sprintsApiHttpClient)
    {
        _issueRepository = issueRepository;
        _projectRepository = projectRepository;
        _sprintsApiHttpClient = sprintsApiHttpClient;
    }

    public async Task<IEnumerable<IssueDto>> HandleAsync(GetIssues query, CancellationToken cancellationToken = default)
    {
        var documents = _issueRepository.Collection.AsQueryable();

        if (query.ProjectId == null)
            return Enumerable.Empty<IssueDto>();

        var project = await _projectRepository.GetAsync(query.ProjectId, cancellationToken);
        if (project == null) return Enumerable.Empty<IssueDto>();

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
            && (query.HasSprint == null ||
                (query.HasSprint.Value ? !sprintIssueIds!.Contains(p.Id) : sprintIssueIds!.Contains(p.Id))));

        var issues = documents.Where(filter).ToList();

        return issues.Select(p => p.AsDto());
    }
}