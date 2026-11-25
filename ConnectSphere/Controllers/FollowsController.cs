using ConnectSphere.Application.Auth.Commands.Register;
using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Follows.Commands.Follows;
using ConnectSphere.Application.Follows.Commands.Unfollow;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ConnectSphere.API.Controllers
{
    [Route("api/users/{userId}")]
    //[ApiController]
    [Authorize]
    public class FollowsController : ApiControllerBase
    {
        private readonly ILogger<FollowsController> _logger;
        private readonly IMediator _mediator;

        public FollowsController(ILogger<FollowsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }


        /// <summary>
        /// an authenticated user can follow another user.
        /// </summary>
        /// <param name="userId">The unique identifier of target user.</param>
        /// <returns>No content.</returns>
        /// <response code="204">Indicates the follow was created successfully.</response>
        /// <response code="400">If the follower or target user Id is not valid,Or if the user follow themselves.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="404">if target user not found.</response>
        /// <response code="409">If the follower already follow this user.</response>
        /// <response code="500">If an unexcepected error has occured.</response>
        [HttpPost("follow")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Follow(Guid userId)
        {
            
           //We validate the existence of followerId and its format in the JwtEvents
           var followerId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var followCommand = new FollowsCommand(Guid.Parse(followerId),userId);

            var result = await _mediator.Send(followCommand);

            return HandleResult(result);
        }



        /// <summary>
        /// an authenticated user can unfollow another user.
        /// </summary>
        /// <param name="userId">The unique identifier of target user.</param>
        /// <returns>No content.</returns>
        /// <response code="204">Indicates the follow was created successfully.</response>
        /// <response code="400">If the follower or target user Id is not valid,Or if the user unfollow themselves.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="404">if target user not found.</response>
        /// <response code="500">If an unexcepected error has occured.</response>
        [HttpPost("unfollow")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Unfollow(Guid userId)
        {

            //We validate the existence of followerId and its format in the JwtEvents
            var followerId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var unfollowCommand = new UnfollowCommand(Guid.Parse(followerId), userId);

            var result = await _mediator.Send(unfollowCommand);

            return HandleResult(result);
        }
    }
}
