using Xunit;

namespace Project.RestApi.IntegrationTest
{
    [CollectionDefinition(nameof(TestFixtureCollection))]
    public class TestFixtureCollection : ICollectionFixture<TestFixture>
    {
    }
}