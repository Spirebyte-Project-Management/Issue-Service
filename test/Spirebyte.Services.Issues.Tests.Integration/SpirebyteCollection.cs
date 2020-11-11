using Spirebyte.Services.Issues.API;
using Spirebyte.Services.Issues.Tests.Shared.Factories;
using Xunit;

namespace Spirebyte.Services.Issues.Tests.Integration
{
    [CollectionDefinition("Spirebyte collection")]
    public class SpirebyteCollection : ICollectionFixture<SpirebyteApplicationFactory<Program>>
    {
    }
}