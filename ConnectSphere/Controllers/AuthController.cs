using ConnectSphere.Application.Auth.Commands.Login;
using ConnectSphere.Application.Auth.Commands.Register;
using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Features.Auth.Commands.ConfirmEmail;
using ConnectSphere.Application.Features.Auth.Commands.ForgotPassword;
using ConnectSphere.Application.Features.Auth.Commands.ResetPasswrd;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static ConnectSphere.Application.Auth.Commands.Register.RegisterCommand;

namespace ConnectSphere.API.Controllers
{
    /// <summary>
    /// Manages user accounts, including registration and authentication.
    /// </summary>
    [Route("api/[controller]")]
    //[ApiController] -- we adding this attribute in the assembly level in the program.cs
    public class AuthController : ApiControllerBase
    {

        private readonly ILogger<AuthController> _logger;
        private readonly IMediator _mediator;

        public AuthController(ILogger<AuthController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }


        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="registerDto">The data required to register a new user.</param>
        /// <returns>A 200 Ok status code if registration is successful.</returns>
        /// <response code="200">Indicates the user was created successfully.</response>
        /// <response code="400">If the registration data is invalid (e.g., duplicate email, password does not meet requirements).</response>
        /// <response code="409">If The email is already used</response>
        /// <response code="500">If an unexcepected error has occured.</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(UserProfileDetailsDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var registerCommand = new RegisterCommand(registerDto);

            var result = await _mediator.Send(registerCommand);
            
            return HandleResult(result);
        }



        /// <summary>
        /// Authenticates a user and provides access and refresh tokens.
        /// </summary>
        /// <param name="loginDto">The user's login credentials.</param>
        /// <returns>An object containing the JWT access token and refresh token.</returns>
        /// <response code="200">Returns the access and refresh tokens.</response>
        /// <response code="401">If the credentials are incorrect.</response>
        /// <response code="400">If the Login data are not valid form.</response>
        /// <response code="500">If an unexcepected error has occured.</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var loginCommand = new LoginCommand(loginDto);

            var result = await _mediator.Send(loginCommand);

            return HandleResult(result);
        }


        /// <summary>
        /// Confirms a user's email address using the token sent during registration.
        /// </summary>
        /// <param name="userId">The ID of the user whose email is being confirmed.</param>
        /// <param name="token">The email confirmation token.</param>
        /// <returns>A 204 No Content status code if confirmation is successful.</returns>
        /// <response code="204">Indicates the email was confirmed successfully.</response>
        /// <response code="400">If the userId or token is missing or invalid.</response>
        /// <response code="404">If the user is not found.</response>
        /// <response code="409">If the email has already been confirmed.</response>
        [HttpGet("confirm-email")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
        {
            var command = new ConfirmEmailCommand(userId, token);

            var result = await _mediator.Send(command);

            return HandleResult(result);
        }


        /// <summary>
        /// Sends a password reset link to the user's registered email address.
        /// </summary>
        /// <param name="command">The request containing the user's email address.</param>
        /// <returns>A 204 No Content status code regardless of whether the email exists.</returns>
        /// <response code="204">Reset link sent (or email not found — same response for security).</response>
        /// <response code="400">If the email is missing or not a valid email format.</response>
        [HttpPost("forgot-password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command)
        {
            var result = await _mediator.Send(command);
            return HandleResult(result);
        }


        /// <summary>
        /// Resets the user's password using the token received via email.
        /// </summary>
        /// <param name="command">The request containing email, token, new password, and confirmation.</param>
        /// <returns>A 204 No Content status code if the password was reset successfully.</returns>
        /// <response code="204">Password was reset successfully.</response>
        /// <response code="400">If the token is invalid, expired, or passwords do not match.</response>
        [HttpPost("reset-password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
        {
            var result = await _mediator.Send(command);
            return HandleResult(result);
        }
    }
}
