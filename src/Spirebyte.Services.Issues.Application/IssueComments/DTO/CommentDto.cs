using System;
using System.Collections.Generic;
using Spirebyte.Services.Issues.Core.Entities;

namespace Spirebyte.Services.Issues.Application.IssueComments.DTO;

public class CommentDto
{
    public string Id { get; set; }
    public string IssueId { get; set; }
    public string ProjectId { get; set; }

    public Guid AuthorId { get; set; }
    public string Body { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }

    public IEnumerable<Reaction> Reactions { get; set; }
}