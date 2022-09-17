using System;
using Spirebyte.Framework.Tests.Shared.Fixtures;
using Spirebyte.Services.Issues.Infrastructure.Mongo.Documents;
using Xunit;

namespace Spirebyte.Services.Issues.Tests.Integration;

[Collection(nameof(SpirebyteCollection))]
public class TestBase : IDisposable
{
    protected readonly MongoDbFixture<CommentDocument, string> CommentsMongoDbFixture;
    protected readonly MongoDbFixture<HistoryDocument, Guid> HistoryMongoDbFixture;
    protected readonly MongoDbFixture<IssueDocument, string> IssuesMongoDbFixture;
    protected readonly MongoDbFixture<ProjectDocument, string> ProjectsMongoDbFixture;
    protected readonly MongoDbFixture<UserDocument, Guid> UsersMongoDbFixture;

    public TestBase(
        MongoDbFixture<CommentDocument, string> commentsMongoDbFixture,
        MongoDbFixture<HistoryDocument, Guid> historyMongoDbFixture,
        MongoDbFixture<IssueDocument, string> issuesMongoDbFixture,
        MongoDbFixture<ProjectDocument, string> projectsMongoDbFixture,
        MongoDbFixture<UserDocument, Guid> usersMongoDbFixture)
    {
        CommentsMongoDbFixture = commentsMongoDbFixture;
        HistoryMongoDbFixture = historyMongoDbFixture;
        IssuesMongoDbFixture = issuesMongoDbFixture;
        ProjectsMongoDbFixture = projectsMongoDbFixture;
        UsersMongoDbFixture = usersMongoDbFixture;
    }
    
    public void Dispose()
    {
        CommentsMongoDbFixture.Dispose();
        HistoryMongoDbFixture.Dispose();
        IssuesMongoDbFixture.Dispose();
        ProjectsMongoDbFixture.Dispose();
        UsersMongoDbFixture.Dispose();
    }
}