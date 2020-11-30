using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Spirebyte.Services.Issues.Application.DTO;
using Spirebyte.Services.Issues.Application.Queries;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spirebyte.Services.Issues.Infrastructure.Mongo.Queries.Handler
{
    internal sealed class GetIssuesByIdsHandler : IQueryHandler<GetIssuesByIds, IEnumerable<IssueDto>>
    {
        private readonly IMongoRepository<IssueDocument, Guid> _repository;


        public GetIssuesByIdsHandler(IMongoRepository<IssueDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<IssueDto>> HandleAsync(GetIssuesByIds query)
        {
            var documents = _repository.Collection.AsQueryable();

            var issues = await documents.Where(u => query.IssueIds.Contains(u.Id)).ToListAsync();

            return issues.Select(p => p.AsDto());
        }
    }
}
