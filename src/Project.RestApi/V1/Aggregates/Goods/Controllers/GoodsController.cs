using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Application.Aggregates.Goods.Commands.CreateGood;
using Project.Application.Aggregates.Goods.Commands.DeleteGood;
using Project.Application.Aggregates.Goods.Commands.UpdateGood;
using Project.Application.Aggregates.Goods.Queries.GetGoodByName;
using Project.Application.Aggregates.Goods.Queries.GetGoodCollection;
using Project.RestApi.V1.Aggregates.Goods.Models;
using Project.RestApi.V1.Models;

namespace Project.RestApi.V1.Aggregates.Goods.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Authorize]
    [Route("rest/api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class GoodsController : ControllerBase
    {
        private readonly IMediator mediator;

        public GoodsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseModel<GoodResponse>>> CreateGood(
            [FromBody] GoodRequest request,
            CancellationToken cancellationToken)
        {
            var command = request.Adapt<CreateGoodCommand>();
            var goodName = await mediator.Send(command, cancellationToken).ConfigureAwait(false);
            var queryResult = await mediator.Send(
                new GetGoodByNameQuery {Name = goodName},
                cancellationToken).ConfigureAwait(false);

            return CreatedAtAction(
                nameof(GetGoodByName),
                new {goodName, version = "1"},
                new ResponseModel<GoodResponse> {Values = queryResult.Adapt<GoodResponse>()});
        }

        [HttpGet]
        public async Task<ActionResult<ResponseCollectionModel<GoodResponse[]>>> GetAllGoods(
            [FromQuery] SearchModel searchModel,
            CancellationToken cancellationToken)
        {
            var query = searchModel.Adapt<GetGoodCollectionQuery>();
            var queryResult = await mediator.Send(query, cancellationToken).ConfigureAwait(false);

            return Ok(new ResponseCollectionModel<GoodResponse>
            {
                Values = queryResult.Result.Select(i => i.Adapt<GoodResponse>()).ToArray(),
                TotalCount = queryResult.TotalCount
            });
        }

        [HttpGet("{goodName}")]
        public async Task<ActionResult<ResponseModel<GoodResponse>>> GetGoodByName(
            string goodName,
            CancellationToken cancellationToken)
        {
            var query = new GetGoodByNameQuery {Name = goodName};
            var queryResult = await mediator.Send(query, cancellationToken).ConfigureAwait(false);

            if (queryResult is null)
                return NotFound();

            return Ok(new ResponseModel<GoodResponse> {Values = queryResult.Adapt<GoodResponse>()});
        }

        [HttpPut("{goodName}")]
        public async Task<ActionResult> UpdateGood(
            string goodName,
            GoodRequest request,
            CancellationToken cancellationToken)
        {
            var command = request.Adapt<UpdateGoodCommand>();
            command.CurrentName = goodName;

            await mediator.Send(command, cancellationToken).ConfigureAwait(false);

            return NoContent();
        }

        [HttpDelete("{goodName}")]
        public async Task<ActionResult> DeleteGood(string goodName, CancellationToken cancellationToken)
        {
            await mediator.Send(new DeleteGoodCommand {Name = goodName}, cancellationToken)
                .ConfigureAwait(false);

            return Ok();
        }
    }
}