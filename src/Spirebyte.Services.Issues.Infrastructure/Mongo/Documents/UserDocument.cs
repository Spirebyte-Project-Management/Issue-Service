using Convey.Types;
using System;

namespace Spirebyte.Services.Issues.Infrastructure.Mongo.Documents
{
    internal sealed class UserDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
    }
}
