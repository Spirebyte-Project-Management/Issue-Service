using Spirebyte.Services.Issues.Core.Entities;
using System.Threading.Tasks;

namespace Spirebyte.Services.Issues.Core.Repositories
{
    public interface ICommentRepository
    {
        Task<Comment> GetAsync(string commentId);
        Task<int> GetCommentCountOfIssue(string issueId);
        Task AddAsync(Comment comment);
        Task UpdateAsync(Comment comment);
        Task DeleteAsync(string commentId);
        Task<Comment> GetLatest();
    }
}
