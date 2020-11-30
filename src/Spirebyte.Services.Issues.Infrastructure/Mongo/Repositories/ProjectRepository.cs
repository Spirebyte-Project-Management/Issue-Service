using Convey.Persistence.MongoDB;
using Spirebyte.Services.Issues.Core.Entities;
using Spirebyte.Services.Issues.Core.Repositories;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents.Mappers;
using System;
using System.Threading.Tasks;

namespace Spirebyte.Services.Issues.Infrastructure.Mongo.Repositories
{
    internal sealed class ProjectRepository : IProjectRepository
    {
        private readonly IMongoRepository<ProjectDocument, Guid> _repository;

        public ProjectRepository(IMongoRepository<ProjectDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<string> GetKeyAsync(Guid projectId)
        {
            var project = await _repository.GetAsync(projectId);
            return project.Key;
        }

        public Task<bool> ExistsAsync(Guid id) => _repository.ExistsAsync(c => c.Id == id);
        public Task<bool> ExistsAsync(string projectKey) => _repository.ExistsAsync(c => c.Key == projectKey);
        public Task AddAsync(Project project) => _repository.AddAsync(project.AsDocument());
    }
}
