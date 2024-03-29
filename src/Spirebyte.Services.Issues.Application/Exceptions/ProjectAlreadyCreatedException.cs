﻿using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Issues.Application.Exceptions;

public class ProjectAlreadyCreatedException : AppException
{
    public ProjectAlreadyCreatedException(string projectId)
        : base($"Project with id: {projectId} was already created.")
    {
        ProjectId = projectId;
    }

    public string ProjectId { get; }
}