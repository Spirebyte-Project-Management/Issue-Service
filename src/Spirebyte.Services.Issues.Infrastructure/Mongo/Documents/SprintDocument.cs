using Convey.Types;
using System;

namespace Spirebyte.Services.Issues.Infrastructure.Mongo.Documents
{
    internal sealed class SprintDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
    }
}
