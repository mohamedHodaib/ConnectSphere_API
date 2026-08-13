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
    [Route("api/[controller]")]
    //[ApiController] -- we adding this attribute in the assembly level in the program.cs
    public class FeedController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public FeedController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// an authenticated user can get the feed.
        /// </summary>
        /// <param name="paginationParameters">dto that contains the page number and page size that extracted 
        /// from the query.</param>
        /// <returns>Feed contains detailed posts for the authenticated user.</returns>
        /// <response code="200">Returns the paged result of detailed posts.</response>
        /// <response code="400">If the pagination data is not valid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="500">If an unexcepected error has occured.</response>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(PagedResult<PostDetailsDto>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFeed([FromQuery] PaginationParameters paginationParameters)
        {

            //We validate the existence of userId and its format in the JwtEvents
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var getFeedQuery = new GetFeedQuery(Guid.Parse(userId), paginationParameters);

            
            var result = await _mediator.Send(getFeedQuery);

            return HandlePagedResult(result);
        }
    }
}
