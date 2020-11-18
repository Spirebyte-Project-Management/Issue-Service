using System;
using System.Collections.Generic;
using System.Text;
using Convey.Types;

namespace Spirebyte.Services.Issues.Infrastructure.Mongo.Documents
{
    internal sealed class SprintDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
    }
}
