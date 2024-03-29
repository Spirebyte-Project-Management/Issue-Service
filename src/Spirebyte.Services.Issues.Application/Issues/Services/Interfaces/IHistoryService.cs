﻿using System.Threading.Tasks;
using Spirebyte.Services.Issues.Core.Entities;
using Spirebyte.Services.Issues.Core.Enums;

namespace Spirebyte.Services.Issues.Application.Issues.Services.Interfaces;

public interface IHistoryService
{
    Task SaveHistory(Issue oldIssue, Issue newIssue, HistoryTypes action);
}