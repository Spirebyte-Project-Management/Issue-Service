using FluentAssertions;
using Spirebyte.Services.Issues.Core.Entities;
using Spirebyte.Services.Issues.Core.Entities.Base;
using Spirebyte.Services.Issues.Core.Enums;
using Spirebyte.Services.Issues.Core.Exceptions;
using System;
using Xunit;

namespace Spirebyte.Services.Issues.Tests.Unit.Core.Entities
{
    public class IssueTests
    {
        [Fact]
        public void given_valid_input_issue_should_be_created()
        {
            var issueId = new AggregateId();
            var projectId = new AggregateId();
            var key = "id-1";
            var type = IssueType.Task;
            var status = IssueStatus.TODO;
            var title = "Title";
            var description = "description";
            var storyPoints = 10;

            var issue = new Issue(issueId, key, type, status, title, description, storyPoints, projectId, null, null, DateTime.UtcNow);

            issue.Should().NotBeNull();
            issue.Id.Should().Be(issueId);
            issue.Key.Should().Be(key);
            issue.Type.Should().Be(type);
            issue.Status.Should().Be(status);
            issue.Title.Should().Be(title);
            issue.Description.Should().Be(description);
            issue.StoryPoints.Should().Be(storyPoints);
            issue.ProjectId.Should().Be(projectId);
        }

        [Fact]
        public void given_empty_projectid_issue_should_throw_an_exeption()
        {
            var issueId = new AggregateId();
            var projectId = Guid.Empty;
            var key = "id-1";
            var type = IssueType.Task;
            var status = IssueStatus.TODO;
            var title = "Title";
            var description = "description";
            var storyPoints = 10;

            Action act = () => new Issue(issueId, key, type, status, title, description, storyPoints, projectId, null, null, DateTime.UtcNow);
            act.Should().Throw<InvalidProjectIdException>();
        }

        [Fact]
        public void given_empty_key_issue_should_throw_an_exeption()
        {
            var issueId = new AggregateId();
            var projectId = new AggregateId();
            var type = IssueType.Task;
            var status = IssueStatus.TODO;
            var title = "Title";
            var description = "description";
            var storyPoints = 10;

            Action act = () => new Issue(issueId, null, type, status, title, description, storyPoints, projectId, null, null, DateTime.UtcNow);
            act.Should().Throw<InvalidKeyException>();
        }

        [Fact]
        public void given_empty_title_issue_should_throw_an_exeption()
        {
            var issueId = new AggregateId();
            var projectId = new AggregateId();
            var key = "id-1";
            var type = IssueType.Task;
            var status = IssueStatus.TODO;
            var description = "description";
            var storyPoints = 10;

            Action act = () => new Issue(issueId, key, type, status, null, description, storyPoints, projectId, null, null, DateTime.UtcNow);
            act.Should().Throw<InvalidTitleException>();
        }
    }
}
