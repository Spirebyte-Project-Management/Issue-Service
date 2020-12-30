using Convey.Persistence.MongoDB;
using Spirebyte.Services.Issues.Core.Entities;
using Spirebyte.Services.Issues.Core.Repositories;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents.Mappers;
using System.Threading.Tasks;

namespace Spirebyte.Services.Issues.Infrastructure.Mongo.Repositories
{
    internal sealed class ProjectRepository : IProjectRepository
    {
        private readonly IMongoRepository<ProjectDocument, string> _repository;

        public ProjectRepository(IMongoRepository<ProjectDocument, string> repository)
        {
            _repository = repository;
        }
        public Task<bool> ExistsAsync(string projectId) => _repository.ExistsAsync(c => c.Id == projectId);
        public Task AddAsync(Project project) => _repository.AddAsync(project.AsDocument());
    }
}
