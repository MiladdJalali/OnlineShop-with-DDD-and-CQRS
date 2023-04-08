using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Application.Aggregates.Orders.Commands.CreateOrder;
using Project.Application.Aggregates.Orders.Commands.DeleteOrder;
using Project.Application.Aggregates.Orders.Commands.UpdateOrder;
using Project.Application.Aggregates.Orders.Queries.GetOrderById;
using Project.Application.Aggregates.Orders.Queries.GetOrderCollection;
using Project.RestApi.V1.Aggregates.Orders.Models;
using Project.RestApi.V1.Models;

namespace Project.RestApi.V1.Aggregates.Orders.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Authorize]
    [Route("rest/api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator mediator;

        public OrdersController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseModel<OrderResponse>>> CreateOrder(
            [FromBody] OrderRequest request,
            CancellationToken cancellationToken)
        {
            var command = request.Adapt<CreateOrderCommand>();
            var orderId = await mediator.Send(command, cancellationToken).ConfigureAwait(false);

            var queryResult = await mediator.Send(
                new GetOrderByIdQuery {Id = orderId},
                cancellationToken).ConfigureAwait(false);

            return CreatedAtAction(
                nameof(GetOrderById),
                new {orderId, version = "1"},
                new ResponseModel<OrderResponse> {Values = queryResult.Adapt<OrderResponse>()});
        }

        [HttpGet]
        public async Task<ActionResult<ResponseCollectionModel<OrderResponse[]>>> GetAllOrders(
            [FromQuery] SearchModel searchModel,
            CancellationToken cancellationToken)
        {
            var query = searchModel.Adapt<GetOrderCollectionQuery>();
            var queryResult = await mediator.Send(query, cancellationToken).ConfigureAwait(false);

            return Ok(new ResponseCollectionModel<OrderResponse>
            {
                Values = queryResult.Result.Select(i => i.Adapt<OrderResponse>()).ToArray(),
                TotalCount = queryResult.TotalCount
            });
        }

        [HttpGet("{orderId:guid}")]
        public async Task<ActionResult<ResponseModel<OrderResponse>>> GetOrderById(
            Guid orderId,
            CancellationToken cancellationToken)
        {
            var query = new GetOrderByIdQuery {Id = orderId};
            var queryResult = await mediator.Send(query, cancellationToken).ConfigureAwait(false);

            if (queryResult is null)
                return NotFound();

            return Ok(new ResponseModel<OrderResponse> {Values = queryResult.Adapt<OrderResponse>()});
        }

        [HttpPut("{orderId:guid}")]
        public async Task<ActionResult> UpdateOrder(
            Guid orderId,
            OrderRequest request,
            CancellationToken cancellationToken)
        {
            var command = request.Adapt<UpdateOrderCommand>();
            command.OrderId = orderId;

            await mediator.Send(command, cancellationToken).ConfigureAwait(false);

            return NoContent();
        }

        [HttpDelete("{orderId:guid}")]
        public async Task<ActionResult> DeleteOrder(Guid orderId, CancellationToken cancellationToken)
        {
            await mediator.Send(new DeleteOrderCommand {Id = orderId}, cancellationToken)
                .ConfigureAwait(false);

            return Ok();
        }
    }
}