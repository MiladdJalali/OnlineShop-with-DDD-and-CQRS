using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Project.RestApi.V1.Aggregates.Orders.Models;
using Project.RestApi.V1.Models;

namespace Project.RestApi.IntegrationTest.V1.Aggregates.Orders.Controllers
{
    public partial class OrdersControllerIntegrationTest
    {
        private async Task<ResponseCollectionModel<OrderItemResponse>> GetItems(
            Guid OrderId,
            HttpStatusCode responseStatusCode = HttpStatusCode.OK)
        {
            var response = await client.GetAsync($"{BaseUrl}/{OrderId}/Items").ConfigureAwait(false);

            response.StatusCode.Should().Be(responseStatusCode);

            return responseStatusCode != HttpStatusCode.OK
                ? null
                : await response.ReadAsync<ResponseCollectionModel<OrderItemResponse>>();
        }

        private async Task ChangeStatus(
            Guid OrderId,
            ChangeOrderStatusRequest request,
            HttpStatusCode responseStatusCode = HttpStatusCode.NoContent)
        {
            var response = await client.PutAsJsonAsync($"{BaseUrl}/{OrderId}/ChangeStatus", request);

            response.StatusCode.Should().Be(responseStatusCode);
        }
    }
}