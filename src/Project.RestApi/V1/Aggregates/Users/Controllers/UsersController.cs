using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Application.Aggregates.Users.Commands.CreateUser;
using Project.Application.Aggregates.Users.Commands.DeleteUser;
using Project.Application.Aggregates.Users.Commands.UpdateUser;
using Project.Application.Aggregates.Users.Queries.GetUserByUsername;
using Project.Application.Aggregates.Users.Queries.GetUserCollection;
using Project.RestApi.V1.Aggregates.Users.Models;
using Project.RestApi.V1.Models;

namespace Project.RestApi.V1.Aggregates.Users.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Authorize]
    [Route("rest/api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator mediator;

        public UsersController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseModel<UserResponse>>> CreateUser(
            [FromBody] UserRequest request,
            CancellationToken cancellationToken)
        {
            var command = request.Adapt<CreateUserCommand>();
            var username = await mediator.Send(command, cancellationToken).ConfigureAwait(false);
            var queryResult = await mediator.Send(
                new GetUserByUsernameQuery {Username = username},
                cancellationToken).ConfigureAwait(false);

            return CreatedAtAction(
                nameof(GetUserByUsername),
                new {username, version = "1"},
                new ResponseModel<UserResponse> {Values = queryResult.Adapt<UserResponse>()});
        }

        [HttpGet]
        public async Task<ActionResult<ResponseCollectionModel<UserResponse[]>>> GetAllUsers(
            [FromQuery] SearchModel searchModel,
            CancellationToken cancellationToken)
        {
            var query = searchModel.Adapt<GetUserCollectionQuery>();
            var queryResult = await mediator.Send(query, cancellationToken).ConfigureAwait(false);

            return Ok(new ResponseCollectionModel<UserResponse>
            {
                Values = queryResult.Result.Select(i => i.Adapt<UserResponse>()).ToArray(),
                TotalCount = queryResult.TotalCount
            });
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<ResponseModel<UserResponse>>> GetUserByUsername(
            string username,
            CancellationToken cancellationToken)
        {
            var query = new GetUserByUsernameQuery {Username = username};
            var queryResult = await mediator.Send(query, cancellationToken).ConfigureAwait(false);

            if (queryResult is null)
                return NotFound();

            return Ok(new ResponseModel<UserResponse> {Values = queryResult.Adapt<UserResponse>()});
        }

        [HttpPut("{username}")]
        public async Task<ActionResult> UpdateUser(
            string username,
            UserRequest request,
            CancellationToken cancellationToken)
        {
            var command = request.Adapt<UpdateUserCommand>();
            command.CurrentUsername = username;

            await mediator.Send(command, cancellationToken).ConfigureAwait(false);

            return NoContent();
        }

        [HttpDelete("{username}")]
        public async Task<ActionResult> DeleteUser(string username, CancellationToken cancellationToken)
        {
            await mediator.Send(new DeleteUserCommand {Username = username}, cancellationToken)
                .ConfigureAwait(false);

            return Ok();
        }
    }
}