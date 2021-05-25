using Spirebyte.Services.Issues.Core.Entities;
using Spirebyte.Services.Issues.Core.Enums;
using System.Threading.Tasks;

namespace Spirebyte.Services.Issues.Application.Services.Interfaces
{
    public interface IHistoryService
    {
        Task SaveHistory(Issue oldIssue, Issue newIssue, HistoryTypes action);
    }
}
