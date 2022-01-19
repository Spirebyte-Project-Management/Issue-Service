using System;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Spirebyte.Services.Issues.Core.Entities;
using Spirebyte.Services.Issues.Core.Repositories;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents.Mappers;

namespace Spirebyte.Services.Issues.Infrastructure.Mongo.Repositories;

internal sealed class HistoryRepository : IHistoryRepository
{
    private readonly IMongoRepository<HistoryDocument, Guid> _repository;

    public HistoryRepository(IMongoRepository<HistoryDocument, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<History> GetAsync(Guid historyId)
    {
        var history = await _repository.GetAsync(historyId);
        return history.AsEntity();
    }

    public Task AddAsync(History history)
    {
        return _repository.AddAsync(history.AsDocument());
    }
}