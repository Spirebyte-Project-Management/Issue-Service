﻿using System;
using Convey.CQRS.Commands;
using Spirebyte.Services.Issues.Application.Events;
using Spirebyte.Services.Issues.Application.Exceptions;
using Spirebyte.Services.Issues.Application.Services.Interfaces;
using Spirebyte.Services.Issues.Core.Entities;
using Spirebyte.Services.Issues.Core.Repositories;
using System.Threading.Tasks;

namespace Spirebyte.Services.Issues.Application.Commands.Handlers
{
    // Simple wrapper
    internal sealed class CreateIssueHandler : ICommandHandler<CreateIssue>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IIssueRepository _issueRepository;
        private readonly IMessageBroker _messageBroker;

        public CreateIssueHandler(IProjectRepository projectRepository, IIssueRepository issueRepository,
            IMessageBroker messageBroker)
        {
            _projectRepository = projectRepository;
            _issueRepository = issueRepository;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(CreateIssue command)
        {
            if (!(await _projectRepository.ExistsAsync(command.ProjectId)))
            {
                throw new ProjectNotFoundException(command.ProjectId);
            }

            if (command.EpicId != Guid.Empty && !(await _issueRepository.ExistsAsync(command.EpicId)))
            {
                throw new EpicNotFoundException(command.EpicId);
            }

            var projectKey = await _projectRepository.GetKeyAsync(command.ProjectId);
            var issueCount = await _issueRepository.GetIssueCountOfProject(command.ProjectId);
            var issueKey = $"{projectKey}-{issueCount + 1}";


            var issue = new Issue(command.IssueId, issueKey, command.Type, command.Status, command.Title, command.Description, command.StoryPoints, command.ProjectId, command.EpicId, command.Assignees, command.LinkedIssues, command.CreatedAt);
            await _issueRepository.AddAsync(issue);
            await _messageBroker.PublishAsync(new IssueCreated(issue.Id, issue.Key, issue.ProjectId));
        }
    }
}
