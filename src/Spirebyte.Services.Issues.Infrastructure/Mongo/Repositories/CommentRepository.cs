using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Spirebyte.Services.Issues.Core.Entities;
using Spirebyte.Services.Issues.Core.Repositories;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents.Mappers;

namespace Spirebyte.Services.Issues.Infrastructure.Mongo.Repositories;

internal sealed class CommentRepository : ICommentRepository
{
    private readonly IMongoRepository<CommentDocument, string> _repository;

    public CommentRepository(IMongoRepository<CommentDocument, string> repository)
    {
        _repository = repository;
    }

    public async Task<Comment> GetAsync(string commentId)
    {
        var comment = await _repository.GetAsync(commentId);
        return comment.AsEntity();
    }

    public async Task<int> GetCommentCountOfIssue(string issueId)
    {
        var documents = _repository.Collection.AsQueryable();

        return await documents.CountAsync(c => c.IssueId == issueId);
    }

    public async Task AddAsync(Comment comment)
    {
        await _repository.AddAsync(comment.AsDocument());
    }

    public async Task UpdateAsync(Comment comment)
    {
        await _repository.UpdateAsync(comment.AsDocument());
    }

    public async Task DeleteAsync(string commentId)
    {
        await _repository.DeleteAsync(commentId);
    }

    public async Task<Comment> GetLatest()
    {
        var documents = _repository.Collection.AsQueryable();

        var comment = await documents.OrderByDescending(c => c.CreatedAt).FirstOrDefaultAsync();
        return comment.AsEntity();
    }
}