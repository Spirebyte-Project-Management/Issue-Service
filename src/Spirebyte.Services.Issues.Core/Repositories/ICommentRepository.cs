using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Spirebyte.Services.Issues.Core.Entities;

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
