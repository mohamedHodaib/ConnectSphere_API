using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Profiles.Queries.GetOwnProfileInfo;
using ConnectSphere.Application.Features.Profiles.Queries.GetPublicProfileInfo;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ConnectSphere.Application.Users.Queries.DeleteUser;

namespace ConnectSphere.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ApiControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IMediator _mediator;

        public UsersController(ILogger<UsersController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        /// <summary>
        /// An admin can delete any user.
        /// </summary>
        /// <returns>No Content.</returns>
        /// <response code="204">Indicate a user deleted successfully.</response>
        /// <response code="401">If the user is not authenticated (missing or invalid JWT).</response>
        /// <response code="404">If the user is not found.</response>
        /// <response code="403">If a non-admin user attempts to delete a user.</response>
        /// <response code="500">If an unexcepected error has occured.</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid id)
        {

            var getPublicProfileInfoQuery = new DeleteUserCommand(id);

            var result = await _mediator.Send(getPublicProfileInfoQuery);

            return HandleResult(result);
        }
    }
}
