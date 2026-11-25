using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Features.Notifications.Queries.GetUserNotifications;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ConnectSphere.Application.Notifications.Commands.MarkAsRead;
using ConnectSphere.Application.Notifications.Commands.MarkAllAsRead;
using ConnectSphere.Application.Notifications.Queries.GetUnreadCount;

namespace ConnectSphere.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public NotificationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// an authenticated user can get their notifications.
        /// </summary>
        /// <param name="paginationParameters">dto that contains the page number and page size that extracted 
        /// from the query.</param>
        /// <param name="unreadOnly">if true, returns only unread notifications.</param>
        /// <returns>Paged result of the authenticated user's notifications.</returns>
        /// <response code="200">Returns the paged result of notifications.</response>
        /// <response code="400">If the pagination data is not valid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="500">If an unexcepected error has occured.</response>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(PagedResult<NotificationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserNotifications([FromQuery] PaginationParameters paginationParameters
            , [FromQuery] bool unreadOnly = false)
        {
            //We validate the existence of userId and its format in the JwtEvents
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var getUserNotificationsQuery = new GetUserNotificationsQuery(Guid.Parse(userId)
                , unreadOnly
                , paginationParameters);

            var result = await _mediator.Send(getUserNotificationsQuery);
            return HandlePagedResult(result);
        }


        /// <summary>
        /// an authenticated user can get their unread notifications count.
        /// </summary>
        /// <returns>The number of unread notifications for the authenticated user.</returns>
        /// <response code="200">Returns the unread notifications count.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="500">If an unexcepected error has occured.</response>
        [HttpGet("unread-count")]
        [Authorize]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUnreadNotificationsCount()
        {
            //We validate the existence of userId and its format in the JwtEvents
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var getUnreadCountQuery = new GetUnreadNotificationsCountQuery(Guid.Parse(userId));
            var result = await _mediator.Send(getUnreadCountQuery);
            return Ok(result);
        }


        /// <summary>
        /// an authenticated user can mark a specific notification as read.
        /// </summary>
        /// <param name="id">the id of the notification to mark as read.</param>
        /// <response code="204">If the notification was marked as read successfully.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="500">If an unexcepected error has occured.</response>
        [HttpPost("read-all")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> MarkAllAsRead()
        {
            //We validate the existence of userId and its format in the JwtEvents
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var markAllAsReadCommand = new MarkAllNotificationsAsReadCommand(Guid.Parse(userId));
            var result = await _mediator.Send(markAllAsReadCommand);
            return HandleResult(result);
        }


        /// <summary>
        /// an authenticated user can mark a specific notification as read.
        /// </summary>
        /// <param name="id">the id of the notification to mark as read.</param>
        /// <response code="204">If the notification was marked as read successfully.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="404">If the notification does not exist.</response>
        /// <response code="500">If an unexcepected error has occured.</response>
        [HttpPost("{id}/read")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> MarkAsRead(Guid id)
        {
            //We validate the existence of userId and its format in the JwtEvents
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var markAsReadCommand = new MarkNotificationAsReadCommand(id, Guid.Parse(userId));
            var result = await _mediator.Send(markAsReadCommand);
            return HandleResult(result);
        }
    }
}
