﻿using System.Threading.Tasks;

namespace Spirebyte.Services.Issues.Application.Clients.Interfaces;

public interface ISprintsApiHttpClient
{
    Task<string[]> IssuesWithoutSprintForProject(string projectKey);
}