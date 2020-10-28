using System;
using Convey.Types;

namespace Spirebyte.Services.Issues.Infrastructure.Mongo.Documents
{
    internal sealed class UserDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
    }
}
