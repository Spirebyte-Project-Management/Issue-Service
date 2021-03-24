using System;
using System.Collections.Generic;
using System.Text;
using Convey.Types;
using Spirebyte.Services.Issues.Core.Entities;
using Spirebyte.Services.Issues.Core.Enums;

namespace Spirebyte.Services.Issues.Infrastructure.Mongo.Documents
{
    internal sealed class HistoryDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public string IssueId { get; set; }
        public Guid UserId { get; set; }
        public HistoryTypes Action { get; set; }
        public DateTime CreatedAt { get; set; }
        public Field[] ChangedFields { get; set; }
    }
}
