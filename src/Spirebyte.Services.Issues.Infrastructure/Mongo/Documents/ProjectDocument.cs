using Convey.Types;
using System;

namespace Spirebyte.Services.Issues.Infrastructure.Mongo.Documents
{
    internal sealed class ProjectDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
    }
}
