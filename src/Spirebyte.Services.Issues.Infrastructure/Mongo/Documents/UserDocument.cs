using System;
using Spirebyte.Framework.Shared.Types;

namespace Spirebyte.Services.Issues.Infrastructure.Mongo.Documents;

public sealed class UserDocument : IIdentifiable<Guid>
{
    public Guid Id { get; set; }
}