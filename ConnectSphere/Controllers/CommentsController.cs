using ConnectSphere.Application.Comments.Commands.AddComment;
using ConnectSphere.Application.Comments.Queries.ViewComments;
using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Feed.Queries.GetFeed;
using ConnectSphere.Application.Follows.Commands.Follows;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ConnectSphere.API.Controllers
{
    [Route("api/Posts/{postId}/[controller]")]
    [ApiController]
    public class CommentsController : ApiControllerBase
    {
        private readonly ILogger<CommentsController> _logger;
        private readonly IMediator _mediator;

        public CommentsController(ILogger<CommentsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }


        /// <summary>
        /// an authenticated user can comment on any post.
        /// </summary>
        /// <param name="addCommentDto">Dto that contain the content of the comment.</param>
        /// <param name="postId">The unique identifier of the post.</param>
        /// <returns>Comment details dto.</returns>
        /// <response code="200">Indicates the comment was created successfully.</response>
        /// <response code="400">If the add comment dto data is invalid(postId is empty or not guid,content is empty or null).</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="404">If the Post that user comment on not found.</response>
        /// <response code="500">If an unexcepected error has occured.</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(CommentDetailsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddComment(Guid postId,AddCommentDto addCommentDto)
        {

            //We validate the existence of followerId and its format in the JwtEvents
            var commentedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var followCommand = new AddCommentCommand(Guid.Parse(commentedUserId),postId,addCommentDto);

            var result = await _mediator.Send(followCommand);

            return HandleResult(result);
        }


        /// <summary>
        /// Any user can view comments on any post.
        /// </summary>
        /// <param name="paginationParameters">dto that contains the page number and page size that extracted 
        /// from the query.</param>
        /// <param name="postId">The unique identifier of the post.</param>
        /// <returns>Feed contains detailed comments for the authenticated user.</returns>
        /// <response code="200">Returns the paged result of detailed comments.</response>
        /// <response code="400">If the pagination data is not valid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="404">If the Post that comments belong to not found.</response>
        /// <response code="500">If an unexcepected error has occured.</response>
        [HttpGet]
        [ProducesResponseType(typeof(CommentDetailsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ViewComments(Guid postId
            , [FromQuery] PaginationParameters paginationParameters)
        {

            var viewCommentsQuery = new ViewCommentsQuery(postId, paginationParameters);

            var result = await _mediator.Send(viewCommentsQuery);

            return HandlePagedResult(result);
        }
    }
}
