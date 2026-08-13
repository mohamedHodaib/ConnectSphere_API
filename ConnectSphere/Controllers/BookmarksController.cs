using ConnectSphere.Application.Features.Bookmarks.Commands.CreateBookmark;
using ConnectSphere.Application.Features.Bookmarks.Commands.DeleteBookmark;
using ConnectSphere.Application.Features.Bookmarks.Queries.GetUserBookmarks;
using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Features.Bookmarks.Commands.CreateBookmark;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ConnectSphere.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookmarksController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public BookmarksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// an authenticated user can get their bookmarked posts.
        /// </summary>
        /// <param name="paginationParameters">dto that contains the page number and page size that extracted 
        /// from the query.</param>
        /// <returns>Paged result of the authenticated user's bookmarked posts.</returns>
        /// <response code="200">Returns the paged result of bookmarked posts.</response>
        /// <response code="400">If the pagination data is not valid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="500">If an unexcepected error has occured.</response>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(PagedResult<PostDetailsDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserBookmarks([FromQuery] PaginationParameters paginationParameters)
        {
            //We validate the existence of userId and its format in the JwtEvents
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var getUserBookmarksQuery = new GetUserBookmarksQuery(Guid.Parse(userId), paginationParameters);
            var result = await _mediator.Send(getUserBookmarksQuery);
            return HandlePagedResult(result);
        }

        /// <summary>
        /// an authenticated user can bookmark a post.
        /// </summary>
        /// <param name="postId">the id of the post to bookmark.</param>
        /// <response code="204">If the post was bookmarked successfully.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="404">If the post does not exist.</response>
        /// <response code="409">If the post is already bookmarked.</response>
        /// <response code="500">If an unexcepected error has occured.</response>
        [HttpPost("{postId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateBookmark(Guid postId)
        {
            //We validate the existence of userId and its format in the JwtEvents
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var createBookmarkCommand = new CreateBookmarkCommand(Guid.Parse(userId), postId);
            var result = await _mediator.Send(createBookmarkCommand);
            return HandleResult(result);
        }

        /// <summary>
        /// an authenticated user can remove a bookmark from a post.
        /// </summary>
        /// <param name="postId">the id of the post to remove the bookmark from.</param>
        /// <response code="204">If the bookmark was removed successfully.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="404">If no bookmark exists for this post.</response>
        /// <response code="500">If an unexcepected error has occured.</response>
        [HttpDelete("{postId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteBookmark(Guid postId)
        {
            //We validate the existence of userId and its format in the JwtEvents
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var deleteBookmarkCommand = new DeleteBookmarkCommand(Guid.Parse(userId), postId);
            var result = await _mediator.Send(deleteBookmarkCommand);
            return HandleResult(result);
        }
    }
}