using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Models;
using ConnectSphere.Application.Posts.Commands.CreatePost;
using ConnectSphere.Application.Posts.Commands.DeletePost;
using ConnectSphere.Application.Posts.Commands.UpdatePost;
using ConnectSphere.Application.Posts.Queries.GetPost;
using ConnectSphere.Application.Reactions.Queries.GetUserReactedPosts;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ConnectSphere.API.Controllers
{
    [Route("api/[controller]")]
    //[ApiController] -- we adding this attribute in the assembly level in the program.cs
    public class PostsController : ApiControllerBase
    {
        private readonly ILogger<PostsController> _logger;
        private readonly IMediator _mediator;

        public PostsController(ILogger<PostsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }




        /// <summary>
        /// an authenticated user can get posts they have liked (reacted to, regardless of reaction type).
        /// </summary>
        /// <param name="paginationParameters">dto that contains the page number and page size that extracted 
        /// from the query.</param>
        /// <returns>Paged result of posts the authenticated user has liked.</returns>
        /// <response code="200">Returns the paged result of liked posts.</response>
        /// <response code="400">If the pagination data is not valid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="500">If an unexcepected error has occured.</response>
        [HttpGet("reacted")]
        [Authorize]
        [ProducesResponseType(typeof(PagedResult<PostDetailsDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserReactedPosts([FromQuery] PaginationParameters paginationParameters)
        {
            //We validate the existence of userId and its format in the JwtEvents
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var getUserReactedPostsQuery = new GetUserReactedPostsQuery(Guid.Parse(userId)
                , paginationParameters);
            var result = await _mediator.Send(getUserReactedPostsQuery);
            return HandlePagedResult(result);
        }


        /// <summary>
        /// An authenticated user creates a new post.
        /// </summary>
        /// <param name="dto">The data required to create a new post.</param>
        /// <returns>A 201 Created status code with the location of the new post.</returns>
        /// <response code="201">Indicates the post was created successfully.</response>
        /// <response code="400">If the data is invalid (handled by FluentValidation).</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="500">If an unexcepected error has occured.</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(PostDetailsDto),StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateNewPost([FromForm] CreatePostDto dto)
        {

            //We validate the existence of followerId and its format in the JwtEvents
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var CreatePostCommand = new AddCommentCommand(Guid.Parse(userId),dto);

            var result = await _mediator.Send(CreatePostCommand);

            return HandleCreatedResult(result, nameof(GetPostById), data => new { data.Id });
        }

        /// <summary>
        /// Any user can view a single post by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the post.</param>
        /// <returns>The details of the requested post.</returns>
        /// <response code="200">Returns the post details.</response>
        /// <response code="400">If the provided ID is invalid.</response>
        /// <response code="404">If the post is not found.</response>
        /// <response code="500">If an unexcepected error has occured.</response>
        [HttpGet("{id}",Name = "GetPostById")]
        [ProducesResponseType(typeof(PostDetailsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPostById(Guid id)
        {
           var result = await _mediator.Send(new GetPostQuery(id));

           return HandleResult(result);
        }

        /// <summary>
        /// An authenticated user updates their own post.
        /// </summary>
        /// <param name="id">The unique identifier of the post to update.</param>
        /// <param name="dto">The new content for the post.</param>
        /// <returns>No content.</returns>
        /// <response code="204">Indicates the post was updated successfully.</response>
        /// <response code="400">If the data is invalid (handled by FluentValidation).</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user attempts to update a post they do not own.</response>
        /// <response code="404">If the post is not found.</response>
        /// <response code="500">If an unexpected error has occurred.</response>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePost(Guid id,[FromBody] UpdatePostDto dto)
        {

            //We validate the existence of followerId and its format in the JwtEvents
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var UpdatePostCommand = new UpdatePostCommand(Guid.Parse(userId),id, dto);

            var result = await _mediator.Send(UpdatePostCommand);

            return HandleResult(result);
        }


        /// <summary>
        /// Deletes a post. Users can delete their own; Admins can delete any.
        /// </summary>
        /// <param name="id">The unique identifier of the post to delete.</param>
        /// <returns>No content.</returns>
        /// <response code="204">Indicates the post was deleted successfully.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If a non-admin user attempts to delete another user's post.</response>
        /// <response code="404">If the post is not found.</response>
        /// <response code="500">If an unexpected error has occurred.</response>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePost(Guid id)
        {

            //We validate the existence of followerId and its format in the JwtEvents
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var isAdmin = User.IsInRole("Admin");

            var DeletePostCommand = new DeletePostCommand(isAdmin,Guid.Parse(userId), id);

            var result = await _mediator.Send(DeletePostCommand);

            return HandleResult(result);
        }
    }
}
