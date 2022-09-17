using System;
using System.Collections.Generic;
using Spirebyte.Framework.Shared.Types;
using Spirebyte.Services.Issues.Core.Entities;

namespace Spirebyte.Services.Issues.Infrastructure.Mongo.Documents;

public sealed class CommentDocument : IIdentifiable<string>
{
    public string IssueId { get; set; }
    public string ProjectId { get; set; }

    public Guid AuthorId { get; set; }
    public string Body { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }


    public IEnumerable<Reaction> Reactions { get; set; }
    public string Id { get; set; }
}