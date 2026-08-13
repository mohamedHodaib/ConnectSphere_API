using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Recommendations.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ConnectSphere.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RecommendationsController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public RecommendationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// An authenticated user can get follow suggestions.
        /// </summary>
        /// <param name="paginationParameters">Pagination parameters extracted from the query.</param>
        /// <returns>Follow suggestions containing public profile info for the authenticated user.</returns>
        /// <response code="200">Returns the paged result of follow suggestions.</response>
        /// <response code="400">If the pagination data is not valid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="500">If an unexpected error has occured.</response>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(PagedResult<PublicProfileInfoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFollowSuggestions([FromQuery] PaginationParameters paginationParameters)
        {
            //We validate the existence of userId and its format in the JwtEvents
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var getUserSuggestionsQuery = new GetUserSuggestionsQuery(Guid.Parse(userId), paginationParameters);

            var result = await _mediator.Send(getUserSuggestionsQuery);

            return HandlePagedResult(result);
        }
    }
}
