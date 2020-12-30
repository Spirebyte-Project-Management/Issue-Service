using Convey.Types;

namespace Spirebyte.Services.Issues.Infrastructure.Mongo.Documents
{
    internal sealed class ProjectDocument : IIdentifiable<string>
    {
        public string Id { get; set; }
    }
}
