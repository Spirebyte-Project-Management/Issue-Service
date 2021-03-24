using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Spirebyte.Services.Issues.Core.Entities;
using Spirebyte.Services.Issues.Core.Enums;

namespace Spirebyte.Services.Issues.Application.Services.Interfaces
{
    public interface IHistoryService
    {
        Task SaveHistory(Issue oldIssue, Issue newIssue, HistoryTypes action);
    }
}
