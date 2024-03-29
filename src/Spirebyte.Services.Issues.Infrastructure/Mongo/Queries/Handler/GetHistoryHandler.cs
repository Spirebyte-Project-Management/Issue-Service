﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using Spirebyte.Framework.DAL.MongoDb.Interfaces;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Services.Issues.Application.Issues.DTO;
using Spirebyte.Services.Issues.Application.Issues.Queries;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents.Mappers;

namespace Spirebyte.Services.Issues.Infrastructure.Mongo.Queries.Handler;

internal sealed class GetHistoryHandler : IQueryHandler<GetHistory, IEnumerable<HistoryDto>>
{
    private readonly IMongoRepository<HistoryDocument, Guid> _historyRepository;
    private readonly IMongoRepository<IssueDocument, string> _issueRepository;

    public GetHistoryHandler(IMongoRepository<HistoryDocument, Guid> historyRepository,
        IMongoRepository<IssueDocument, string> issueRepository)
    {
        _historyRepository = historyRepository;
        _issueRepository = issueRepository;
    }

    public async Task<IEnumerable<HistoryDto>> HandleAsync(GetHistory query,
        CancellationToken cancellationToken = default)
    {
        var documents = _historyRepository.Collection.AsQueryable();

        var issue = await _issueRepository.GetAsync(query.IssueId, cancellationToken);
        if (query.IssueId != null && issue == null) return Enumerable.Empty<HistoryDto>();


        var filter = new Func<HistoryDocument, bool>(p =>
            query.IssueId == null || p.IssueId == query.IssueId);

        var history = documents.Where(filter).ToList();

        return history.Select(p => p.AsDto());
    }
}