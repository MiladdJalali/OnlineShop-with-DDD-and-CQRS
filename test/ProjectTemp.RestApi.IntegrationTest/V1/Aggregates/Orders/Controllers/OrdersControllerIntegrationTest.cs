using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Project.Domain.Aggregates.Orders.Enums;
using Project.RestApi.IntegrationTest.SeedHelpers;
using Project.RestApi.V1.Aggregates.Orders.Models;
using Xunit;

namespace Project.RestApi.IntegrationTest.V1.Aggregates.Orders.Controllers
{
    [Collection(nameof(TestFixtureCollection))]
    public partial class OrdersControllerIntegrationTest : BaseControllerIntegrationTest
    {
        public OrdersControllerIntegrationTest(TestFixture testFixture)
            : base(testFixture)
        {
            client = testFixture.Client;
        }

        protected override string BaseUrl { get; } = "/rest/api/v1/Orders";

        private readonly HttpClient client;
        
        [Fact]
        public async Task TestAll_WhenEverythingIsOk_StatusMustBeCorrect()
        {
            // Create
            var createRequest = new OrderRequest
            {
                GoodsName = new[] {GoodDataSeeder.FirstGoodName, GoodDataSeeder.SecondGoodName},
                Description = nameof(OrderRequest.Description)
            };

            var createResponse = await Create<OrderResponse>(createRequest);

            createResponse.Status.Should().Be(OrderStatus.Received.ToString());
            createResponse.Description.Should().Be(createRequest.Description);

            // Update
            var updateRequest = new OrderRequest
            {
                GoodsName = new[] {GoodDataSeeder.FirstGoodName},
                Description = $"{nameof(OrderRequest.Description)}Updated"
            };

            await Update(createResponse.Id.ToString(), updateRequest);

            // GetAll
            var getAllParameters = new
            {
                PageSize = 1,
                PageIndex = 1
            };
            var getAllResponse = await GetAll<OrderResponse>(getAllParameters);

            getAllResponse.Values.Should().HaveCount(1);
            getAllResponse.TotalCount.Should().Be(1);

            // GetItems
            var getOrderItemsResponse = await GetItems(createResponse.Id);
            
            getOrderItemsResponse.TotalCount.Should().Be(2);

            await Delete(createResponse.Id.ToString());
        }

        [Fact]
        public async Task TestPost_WhenTotalPriceLessThanMinimum_StatusMustBeBadRequest()
        {
            var createRequest = new OrderRequest
            {
                GoodsName = new[] {GoodDataSeeder.SecondGoodName},
                Description = nameof(OrderRequest.Description)
            };

            await Create<OrderResponse>(createRequest, HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task TestGetOrderByName_WhenDoesNotExist_StatusMustBeNotFound()
        {
            await GetByIdentifier<OrderResponse>("FakeOrderName", HttpStatusCode.NotFound);
        }
    }
}