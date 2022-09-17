using System.Threading.Tasks;
using Spirebyte.Framework.DAL.MongoDb.Interfaces;
using Spirebyte.Services.Issues.Core.Entities;
using Spirebyte.Services.Issues.Core.Repositories;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents.Mappers;

namespace Spirebyte.Services.Issues.Infrastructure.Mongo.Repositories;

internal sealed class ProjectRepository : IProjectRepository
{
    private readonly IMongoRepository<ProjectDocument, string> _repository;

    public ProjectRepository(IMongoRepository<ProjectDocument, string> repository)
    {
        _repository = repository;
    }

    public Task<bool> ExistsAsync(string projectId)
    {
        return _repository.ExistsAsync(c => c.Id == projectId);
    }

    public Task AddAsync(Project project)
    {
        return _repository.AddAsync(project.AsDocument());
    }
}