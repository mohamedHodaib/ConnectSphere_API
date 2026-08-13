using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Feed.Queries.GetFeed;
using ConnectSphere.Application.Profiles.Commands.UpdateProfileInfo;
using ConnectSphere.Application.Profiles.Queries.GetOwnProfileInfo;
using ConnectSphere.Application.Profiles.Queries.GetOwnSettings;
using ConnectSphere.Application.Features.Profiles.Queries.GetPublicProfileInfo;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ConnectSphere.Application.Profiles.Queries.GetProfilePictureUrl;

namespace ConnectSphere.API.Controllers
{
    [Route("api/[controller]/me")]
    [ApiController]
    public class ProfileController : ApiControllerBase
    {
        private readonly ILogger<ProfileController> _logger;
        private readonly IMediator _mediator;

        public ProfileController(ILogger<ProfileController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        /// <summary>
        /// Retrieves the public profile information of a user by username, including post, follower, and following counts.
        /// </summary>
        /// <param name="username">the username of the profile to retrieve.</param>
        /// <returns>The user's public profile details for display on their profile page.</returns>
        /// <response code="200">Returns the user's public profile information.</response>
        /// <response code="401">If the user is not authenticated (missing or invalid JWT).</response>
        /// <response code="404">If no user exists with the given username.</response>
        /// <response code="500">If an unexcepected error has occured.</response>
        [HttpGet("~/api/profile/{username}")]
        [Authorize]
        [ProducesResponseType(typeof(PublicProfileInfoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProfile(string username)
        {
            var getPublicProfileInfoQuery = new GetPublicProfileInfoQuery(username);
            var result = await _mediator.Send(getPublicProfileInfoQuery);
            return HandleResult(result);
        }

        /// <summary>
        /// Retrieves the profile information of the currently logged-in user.
        /// </summary>
        /// <returns>The user's profile details including private information such as email and other information.</returns>
        /// <response code="200">Returns the user's profile information.</response>
        /// <response code="401">If the user is not authenticated (missing or invalid JWT).</response>
        /// <response code="404">If the user account linked to the token no longer exists.</response>
        /// <response code="500">If an unexcepected error has occured.</response>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(OwnProfileInfoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOwnProfile()
        {

            //We validate the existence of userId and its format in the JwtEvents
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var getOwnProfileInfoCommand = new GetOwnProfileInfoQuery(Guid.Parse(userId));

            var result = await _mediator.Send(getOwnProfileInfoCommand);

            return HandleResult(result);
        }


        /// <summary>
        /// Retrieves the current settings of the logged-in user (profile info, privacy, and notification preferences).
        /// </summary>
        /// <returns>The user's editable settings, used to pre-fill the settings page.</returns>
        /// <response code="200">Returns the user's settings.</response>
        /// <response code="401">If the user is not authenticated (missing or invalid JWT).</response>
        /// <response code="404">If the user account linked to the token no longer exists.</response>
        /// <response code="500">If an unexcepected error has occured.</response>
        [HttpGet("settings")]
        [Authorize]
        [ProducesResponseType(typeof(OwnSettingsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSettings()
        {
            //We validate the existence of userId and its format in the JwtEvents
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var getSettingsQuery = new GetOwnSettingsQuery(Guid.Parse(userId));
            var result = await _mediator.Send(getSettingsQuery);
            return HandleResult(result);
        }

        /// <summary>
        /// Update the profile information of the currently logged-in user.
        /// </summary>
        /// <returns>No Content.</returns>
        /// <response code="204">Indicates the profile was updated successfully.</response>
        /// <response code="401">If the user is not authenticated (missing or invalid JWT).</response>
        /// <response code="404">If the user account linked to the token no longer exists.</response>
        /// <response code="500">If an unexcepected error has occured.</response>
        [HttpPut("settings")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateOwnProfile([FromForm] SettingsDto updateProfileInfoDto)
        {

            //We validate the existence of userId and its format in the JwtEvents
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var updateOwnProfileInfoCommand = new UpdateProfileInfoCommand(Guid.Parse(userId)
                ,updateProfileInfoDto);

            var result = await _mediator.Send(updateOwnProfileInfoCommand);

            return HandleResult(result);
        }


        /// <summary>
        /// an authenticated user can get their profile picture url.
        /// </summary>
        /// <returns>The absolute url of the profile picture, or null if not set.</returns>
        /// <response code="200">Returns the profile picture url.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="404">If the user account linked to the token no longer exists.</response>
        /// <response code="500">If an unexcepected error has occured.</response>
        [HttpGet("picture-url")]
        [Authorize]
        [ProducesResponseType(typeof(ProfilePictureUrlDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOwnProfilePictureUrl()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _mediator.Send(new GetProfilePictureUrlQuery(Guid.Parse(userId)));
            return HandleResult(result);
        }
    }
}
