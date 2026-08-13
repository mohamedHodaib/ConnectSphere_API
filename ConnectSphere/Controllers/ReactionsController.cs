using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Reactions.Commands.AddReaction;
using ConnectSphere.Application.Reactions.Commands.RemoveReaction;
using ConnectSphere.Application.Reactions.Queries.GetUserReactedPosts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ConnectSphere.API.Controllers
{
    [Route("api/posts/{postId}/react")]
    //[ApiController]
    [Authorize]
    public class ReactionsController : ApiControllerBase
    {

        private readonly ILogger<ReactionsController> _logger;
        private readonly IMediator _mediator;

        public ReactionsController(ILogger<ReactionsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }



        /// <summary>
        /// An authenticated user make reaction.
        /// </summary>
        /// <param name="dto">The data required to react to a post.</param>
        /// <returns>No content.</returns>
        /// <response code="204">Indicates the reaction has done successfully.</response>
        /// <response code="400">If the provided data is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="500">If an unexcepected error has occured.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReactToPost(Guid postId,[FromBody] CreateReactionDto dto)
        {

            //We validate the existence of followerId and its format in the JwtEvents
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var addReactionCommand = new AddReactionCommand(Guid.Parse(userId),postId, dto);

            var result = await _mediator.Send(addReactionCommand);

            return HandleResult(result);
        }


        /// <summary>
        /// An authenticated user remove reaction
        /// </summary>
        /// <param name="postId">The id of the post to un react.</param>
        /// <returns>No content.</returns>
        /// <response code="204">Indicates the reaction to the post was deleted successfully.</response>
        /// <response code="400">If the provided data is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="404">If the user does not react to the post.</response>
        /// <response code="500">If an unexcepected error has occured.</response>
        [HttpDelete]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveReaction(Guid postId)
        {

            //We validate the existence of followerId and its format in the JwtEvents
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var removeReactionCommand = new RemoveReactionCommand(Guid.Parse(userId), postId);

            var result = await _mediator.Send(removeReactionCommand);

            return HandleResult(result);
        }
    }
}
