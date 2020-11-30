using Spirebyte.Services.Issues.Core.Entities;

namespace Spirebyte.Services.Issues.Infrastructure.Mongo.Documents.Mappers
{
    internal static class SprintMappers
    {
        public static Sprint AsEntity(this SprintDocument document)
            => new Sprint(document.Id);

        public static SprintDocument AsDocument(this Sprint entity)
            => new SprintDocument
            {
                Id = entity.Id
            };
    }
}
