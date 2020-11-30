using Convey.Persistence.MongoDB;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Spirebyte.Services.Issues.Core.Entities;
using Spirebyte.Services.Issues.Core.Repositories;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents.Mappers;
using System;
using System.Threading.Tasks;

namespace Spirebyte.Services.Issues.Infrastructure.Mongo.Repositories
{
    internal sealed class IssueRepository : IIssueRepository
    {
        private readonly IMongoRepository<IssueDocument, Guid> _repository;

        public IssueRepository(IMongoRepository<IssueDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<Issue> GetAsync(Guid issueId)
        {
            var issue = await _repository.GetAsync(x => x.Id == issueId);

            return issue?.AsEntity();
        }

        public async Task<Issue> GetAsync(string issueKey)
        {
            var issue = await _repository.GetAsync(x => x.Key == issueKey);

            return issue?.AsEntity();
        }

        public async Task<int> GetIssueCountOfProject(Guid projectId)
        {
            var documents = _repository.Collection.AsQueryable();

            return await documents.CountAsync(c => c.ProjectId == projectId);

        }
        public Task AddAsync(Issue issue) => _repository.AddAsync(issue.AsDocument());

        public Task UpdateAsync(Issue issue) => _repository.UpdateAsync(issue.AsDocument());
        public Task DeleteAsync(Guid issueId) => _repository.DeleteAsync(issueId);
    }
}
