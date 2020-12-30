using Spirebyte.Services.Issues.Core.Entities;

namespace Spirebyte.Services.Issues.Infrastructure.Mongo.Documents.Mappers
{
    internal static class ProjectMappers
    {
        public static Project AsEntity(this ProjectDocument document)
            => new Project(document.Id);

        public static ProjectDocument AsDocument(this Project entity)
            => new ProjectDocument
            {
                Id = entity.Id
            };
    }
}
