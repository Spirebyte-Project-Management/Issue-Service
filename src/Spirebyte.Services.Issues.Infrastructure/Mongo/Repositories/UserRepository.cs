using System;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Spirebyte.Services.Issues.Core.Entities;
using Spirebyte.Services.Issues.Core.Repositories;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents.Mappers;

namespace Spirebyte.Services.Issues.Infrastructure.Mongo.Repositories;

internal sealed class UserRepository : IUserRepository
{
    private readonly IMongoRepository<UserDocument, Guid> _repository;

    public UserRepository(IMongoRepository<UserDocument, Guid> repository)
    {
        _repository = repository;
    }

    public Task<bool> ExistsAsync(Guid id)
    {
        return _repository.ExistsAsync(c => c.Id == id);
    }

    public Task AddAsync(User user)
    {
        return _repository.AddAsync(user.AsDocument());
    }
}