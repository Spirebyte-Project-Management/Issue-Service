using FluentAssertions;
using Spirebyte.Services.Issues.Core.Entities;
using Spirebyte.Services.Issues.Core.Exceptions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Spirebyte.Services.Issues.Tests.Unit.Core.Entities
{
    public class CommentTests
    {
        [Fact]
        public void given_valid_input_comment_should_be_created()
        {
            var id = "commentId";
            var projectId = "projectKey";
            var issueId = "issueKey";
            var authorId = Guid.NewGuid();
            var body = "comment body";

            var comment = new Comment(id, issueId, projectId, authorId, body, DateTime.UtcNow, new List<Reaction>());

            comment.Should().NotBeNull();
            comment.Id.Should().Be(id);
            comment.ProjectId.Should().Be(projectId);
            comment.IssueId.Should().Be(issueId);
            comment.AuthorId.Should().Be(authorId);
            comment.Body.Should().Be(body);
        }

        [Fact]
        public void given_empty_id_comment_should_throw_an_exception()
        {
            var id = string.Empty;
            var projectId = "projectKey";
            var issueId = "issueKey";
            var authorId = Guid.NewGuid();
            var body = "comment body";

            Action act = () => new Comment(id, issueId, projectId, authorId, body, DateTime.UtcNow, new List<Reaction>());
            act.Should().Throw<InvalidIdException>();
        }

        [Fact]
        public void given_empty_projectId_comment_should_throw_an_exception()
        {
            var id = "commentKey";
            var projectId = string.Empty;
            var issueId = "issueKey";
            var authorId = Guid.NewGuid();
            var body = "comment body";

            Action act = () => new Comment(id, issueId, projectId, authorId, body, DateTime.UtcNow, new List<Reaction>());
            act.Should().Throw<InvalidProjectIdException>();
        }

        [Fact]
        public void given_empty_issueId_comment_should_throw_an_exception()
        {
            var id = "commentKey";
            var projectId = "projectKey";
            var issueId = string.Empty;
            var authorId = Guid.NewGuid();
            var body = "comment body";

            Action act = () => new Comment(id, issueId, projectId, authorId, body, DateTime.UtcNow, new List<Reaction>());
            act.Should().Throw<InvalidIssueIdException>();
        }

        [Fact]
        public void given_empty_authorId_comment_should_throw_an_exception()
        {
            var id = "commentKey";
            var projectId = "projectKey";
            var issueId = "issueKey";
            var authorId = Guid.Empty;
            var body = "comment body";

            Action act = () => new Comment(id, issueId, projectId, authorId, body, DateTime.UtcNow, new List<Reaction>());
            act.Should().Throw<InvalidAuthorIdException>();
        }

        [Fact]
        public void given_empty_body_comment_should_throw_an_exception()
        {
            var id = "commentKey";
            var projectId = "projectKey";
            var issueId = "issueKey";
            var authorId = Guid.NewGuid();
            var body = string.Empty;

            Action act = () => new Comment(id, issueId, projectId, authorId, body, DateTime.UtcNow, new List<Reaction>());
            act.Should().Throw<InvalidBodyException>();
        }
    }
}
