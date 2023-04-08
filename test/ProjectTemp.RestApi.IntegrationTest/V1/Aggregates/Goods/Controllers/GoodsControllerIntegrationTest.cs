using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Project.RestApi.V1.Aggregates.Goods.Models;
using Xunit;

namespace Project.RestApi.IntegrationTest.V1.Aggregates.Goods.Controllers
{
    [Collection(nameof(TestFixtureCollection))]
    public class GoodsControllerIntegrationTest : BaseControllerIntegrationTest
    {
        public GoodsControllerIntegrationTest(TestFixture testFixture)
            : base(testFixture)
        {
        }

        protected override string BaseUrl { get; } = "/rest/api/v1/Goods";

        [Fact]
        public async Task TestAll_WhenEverythingIsOk_StatusMustBeCorrect()
        {
            // Create
            var createRequest = new GoodRequest
            {
                Name = nameof(GoodRequest.Name),
                Price = 10000,
                Discount = 10,
                IsFragile = true,
                Description = nameof(GoodRequest.Description)
            };

            var createResponse = await Create<GoodResponse>(createRequest);

            createResponse.Name.Should().Be(createRequest.Name);
            createResponse.Price.Should().Be(createRequest.Price);
            createResponse.Discount.Should().Be(createRequest.Discount);
            createResponse.IsFragile.Should().Be(createRequest.IsFragile);
            createResponse.Description.Should().Be(createRequest.Description);

            // Update
            var updateRequest = new GoodRequest
            {
                Name = $"{nameof(GoodRequest.Name)}Updated",
                Price = 20000,
                Discount = 15,
                IsFragile = false,
                Description = $"{nameof(GoodRequest.Description)}Updated"
            };

            await Update(createRequest.Name, updateRequest);

            // GetAll
            var getAllParameters = new
            {
                PageSize = 1,
                PageIndex = 1
            };
            var getAllResponse = await GetAll<GoodResponse>(getAllParameters);

            getAllResponse.Values.Should().HaveCount(1);
            getAllResponse.TotalCount.Should().Be(1);

            // GetByGoodName
            var getGoodByNameResponse = await GetByIdentifier<GoodResponse>(updateRequest.Name);

            getGoodByNameResponse.Name.Should().Be(updateRequest.Name);
            getGoodByNameResponse.Price.Should().Be(updateRequest.Price);
            getGoodByNameResponse.Discount.Should().Be(updateRequest.Discount);
            getGoodByNameResponse.IsFragile.Should().Be(updateRequest.IsFragile);
            getGoodByNameResponse.Description.Should().Be(updateRequest.Description);

            // Delete
            await Delete(updateRequest.Name);
        }

        [Fact]
        public async Task TestGetGoodByName_WhenDoesNotExist_StatusMustBeNotFound()
        {
            await GetByIdentifier<GoodResponse>("FakeGoodName", HttpStatusCode.NotFound);
        }
    }
}