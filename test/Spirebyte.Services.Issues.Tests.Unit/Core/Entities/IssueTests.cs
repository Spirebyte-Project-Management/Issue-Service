using System;
using FluentAssertions;
using Spirebyte.Services.Issues.Core.Entities;
using Spirebyte.Services.Issues.Core.Enums;
using Spirebyte.Services.Issues.Core.Exceptions;
using Xunit;

namespace Spirebyte.Services.Issues.Tests.Unit.Core.Entities;

public class IssueTests
{
    [Fact]
    public void given_valid_input_issue_should_be_created()
    {
        var projectId = "projectKey";
        var epicId = "epicKey";
        var issueId = "issueKey";
        var sprintId = string.Empty;
        var type = IssueType.Task;
        var status = IssueStatus.TODO;
        var title = "Title";
        var description = "description";
        var storyPoints = 10;

        var issue = new Issue(issueId, type, status, title, description, storyPoints, projectId, epicId, sprintId, null,
            null, DateTime.UtcNow);

        issue.Should().NotBeNull();
        issue.Id.Should().Be(issueId);
        issue.Type.Should().Be(type);
        issue.Status.Should().Be(status);
        issue.Title.Should().Be(title);
        issue.Description.Should().Be(description);
        issue.StoryPoints.Should().Be(storyPoints);
        issue.ProjectId.Should().Be(projectId);
    }

    [Fact]
    public void given_empty_projectid_issue_should_throw_an_exception()
    {
        var projectId = string.Empty;
        var epicId = "epicKey";
        var issueId = "issueKey";
        var sprintId = string.Empty;
        var type = IssueType.Task;
        var status = IssueStatus.TODO;
        var title = "Title";
        var description = "description";
        var storyPoints = 10;

        Action act = () => new Issue(issueId, type, status, title, description, storyPoints, projectId, epicId,
            sprintId, null, null, DateTime.UtcNow);
        act.Should().Throw<InvalidProjectIdException>();
    }

    [Fact]
    public void given_empty_id_issue_should_throw_an_exception()
    {
        var projectId = "projectKey";
        var epicId = "epicKey";
        var issueId = string.Empty;
        var sprintId = string.Empty;
        var type = IssueType.Task;
        var status = IssueStatus.TODO;
        var title = "Title";
        var description = "description";
        var storyPoints = 10;

        Action act = () => new Issue(issueId, type, status, title, description, storyPoints, projectId, epicId,
            sprintId, null, null, DateTime.UtcNow);
        act.Should().Throw<InvalidIdException>();
    }

    [Fact]
    public void given_empty_title_issue_should_throw_an_exeption()
    {
        var projectId = "projectKey";
        var epicId = "epicKey";
        var issueId = "issueKey";
        var sprintId = string.Empty;
        var type = IssueType.Task;
        var status = IssueStatus.TODO;
        var title = string.Empty;
        var description = "description";
        var storyPoints = 10;

        Action act = () => new Issue(issueId, type, status, title, description, storyPoints, projectId, epicId,
            sprintId, null, null, DateTime.UtcNow);
        act.Should().Throw<InvalidTitleException>();
    }
}