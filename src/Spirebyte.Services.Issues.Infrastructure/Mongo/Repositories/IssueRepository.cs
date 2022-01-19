using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Spirebyte.Services.Issues.Core.Entities;
using Spirebyte.Services.Issues.Core.Repositories;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents.Mappers;

namespace Spirebyte.Services.Issues.Infrastructure.Mongo.Repositories;

internal sealed class IssueRepository : IIssueRepository
{
    private readonly IMongoRepository<IssueDocument, string> _repository;

    public IssueRepository(IMongoRepository<IssueDocument, string> repository)
    {
        _repository = repository;
    }

    public async Task<Issue> GetAsync(string issueId)
    {
        var issue = await _repository.GetAsync(x => x.Id == issueId);

        return issue?.AsEntity();
    }

    public async Task<int> GetIssueCountOfProject(string projectId)
    {
        var documents = _repository.Collection.AsQueryable();

        return await documents.CountAsync(c => c.ProjectId == projectId);
    }

    public async Task<List<Issue>> GetIssuesWithSprint(string sprintId)
    {
        var documents = _repository.Collection.AsQueryable();

        var issues = await documents.Where(c => c.SprintId == sprintId).ToListAsync();

        return issues.Select(p => p.AsEntity()).ToList();
    }

    public Task AddAsync(Issue issue)
    {
        return _repository.AddAsync(issue.AsDocument());
    }

    public Task<bool> ExistsAsync(string issueId)
    {
        return _repository.ExistsAsync(c => c.Id == issueId);
    }

    public Task UpdateAsync(Issue issue)
    {
        return _repository.UpdateAsync(issue.AsDocument());
    }

    public Task DeleteAsync(string issueId)
    {
        return _repository.DeleteAsync(issueId);
    }

    public async Task<Issue> GetLatest()
    {
        var documents = _repository.Collection.AsQueryable();

        var issue = await documents.OrderByDescending(c => c.CreatedAt).FirstOrDefaultAsync();
        return issue.AsEntity();
    }
}