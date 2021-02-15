using System;
using System.Collections.Generic;
using System.Text;
using Convey.CQRS.Commands;

namespace Spirebyte.Services.Issues.Application.Commands
{
    [Contract]
    public class CreateComment : ICommand
    {
        public string Id { get; private set; }
        public string IssueId { get; private set; }
        public string ProjectId { get; private set; }

        public Guid AuthorId { get; private set; }
        public string Body { get; private set; }

        public CreateComment(string id, string issueId, string projectId, Guid authorId, string body)
        {
            Id = id;
            IssueId = issueId;
            ProjectId = projectId;
            AuthorId = authorId;
            Body = body;
        }
    }
}
