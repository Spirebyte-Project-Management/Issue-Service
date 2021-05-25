using Convey.Types;
using Spirebyte.Services.Issues.Core.Entities;
using System;
using System.Collections.Generic;

namespace Spirebyte.Services.Issues.Infrastructure.Mongo.Documents
{
    internal sealed class CommentDocument : IIdentifiable<string>
    {
        public string Id { get; set; }
        public string IssueId { get; set; }
        public string ProjectId { get; set; }

        public Guid AuthorId { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }


        public IEnumerable<Reaction> Reactions { get; set; }
    }
}
