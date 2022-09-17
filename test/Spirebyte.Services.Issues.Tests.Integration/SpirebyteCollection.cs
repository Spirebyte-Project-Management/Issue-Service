using System;
using Spirebyte.Framework.Tests.Shared.Fixtures;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents;
using Xunit;

[assembly: CollectionBehavior(MaxParallelThreads = 1, DisableTestParallelization = true)]


namespace Spirebyte.Services.Issues.Tests.Integration;

[CollectionDefinition(nameof(SpirebyteCollection), DisableParallelization = true)]
public class SpirebyteCollection : ICollectionFixture<DockerDbFixture>,
    ICollectionFixture<MongoDbFixture<CommentDocument, string>>,
    ICollectionFixture<MongoDbFixture<HistoryDocument, Guid>>,
    ICollectionFixture<MongoDbFixture<IssueDocument, string>>,
    ICollectionFixture<MongoDbFixture<ProjectDocument, string>>,
    ICollectionFixture<MongoDbFixture<UserDocument, Guid>>
{
}