using AutoMapper;
using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Interfaces.Services;
using ConnectSphere.Application.Common.Models;
using ConnectSphere.Application.Common.Models.Errors;
using ConnectSphere.Application.Common.Settings;
using ConnectSphere.Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ConnectSphere.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthService> _logger;
        private readonly IEmailService _emailService;
        private readonly FrontendSettings _frontendSettings;
        private readonly JwtSettings _jwtSettings;

        private User? _user;



        public AuthService(UserManager<User> userManager
            , SignInManager<User> signInManager
            , IMapper mapper
            , ILogger<AuthService> logger
            , IOptions<JwtSettings> jwtSettings
            , IEmailService emailService
            , IOptions<FrontendSettings> frontendSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _logger = logger;
            _jwtSettings = jwtSettings.Value;
            _emailService = emailService;
            _frontendSettings = frontendSettings.Value;
        }

        public async Task<Result<UserProfileDetailsDto>> RegisterAsync(RegisterDto registerDto)
        {

            //Check Uniqueness for email and username.
            var conflictingUser = await _userManager.Users
                .FirstOrDefaultAsync(u => u.NormalizedEmail == registerDto.Email.ToUpper()
                                       || u.NormalizedUserName == registerDto.UserName.ToUpper());

            if (conflictingUser != null)
            {
                // Now you have to check which field caused the conflict
                if (conflictingUser.NormalizedEmail == registerDto.Email.ToUpper())
                    return Result<UserProfileDetailsDto>.Failure(AuthErrors.EmailConflict);

                if (conflictingUser.NormalizedUserName == registerDto.UserName.ToUpper())
                    return Result<UserProfileDetailsDto>.Failure(AuthErrors.UsernameConflict);
            }

            // create User
            var user = new User
            {
               UserName = registerDto.UserName,
               Email = registerDto.Email,
               JointDate = DateTime.UtcNow
            };

            var identityResult = await _userManager.CreateAsync(user, registerDto.Password);

            //Add Role To user
            if (identityResult.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "user");
            }
            else
            {
                var errors = identityResult.Errors
                .Select(e => Error.Validation(e.Code,e.Description))
                .ToList();

                return Result<UserProfileDetailsDto>.Failure(errors);
            }

            //Sending Confirmation email
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var confirmationLink = $"{_frontendSettings.BaseUrl}/confirm-email.html" +
                                   $"?userId={user.Id}&token={encodedToken}";

            await _emailService.SendEmailConfirmationAsync(user.Email!, confirmationLink);


            var userProfileDto = _mapper.Map<UserProfileDetailsDto>(user);

            return Result<UserProfileDetailsDto>.Success(userProfileDto);
        }

        public async Task<Result> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return Result.Failure(AuthErrors.UserNotFound);

            if (user.EmailConfirmed)
                return Result.Failure(AuthErrors.EmailAlreadyConfirmed);

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            Console.WriteLine(decodedToken);
            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            if (!result.Succeeded)
            {
                var errors = result.Errors
                    .Select(e => Error.Validation(e.Code, e.Description))
                    .ToList();
                return Result.Failure(errors);
            }

            return Result.Success();
        }

        public async Task<Result> ValidateAsync(LoginDto dto)
        {

            _user = await _userManager.FindByEmailAsync(dto.Email);

            if (_user == null)
                return Result.Failure(AuthErrors.WrongCredentials);

            if (!_user.EmailConfirmed)
                return Result.Failure(AuthErrors.EmailNotConfirmed);

            var signInResult = await _signInManager
                                        .CheckPasswordSignInAsync(_user, dto.Password, lockoutOnFailure:true);

            if(signInResult.Succeeded)
            {
                return Result.Success();
            }

            if (signInResult.IsLockedOut)
                return Result.Failure(AuthErrors.Lockedout);

            return Result.Failure(AuthErrors.WrongCredentials);
        }


        public async Task<TokenDto> GenerateTokens(bool populateExp)
        {
            //Prepare Access Token

            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();

            //create token descriptor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpireMinutes),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = signingCredentials,
            };


            //create and write the token
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var accessToken = tokenHandler.WriteToken(token);


            //Prepare Refresh Token 
            var refreshToken = GetRefreshToken();

            _user!.RefreshToken = refreshToken;

            if (populateExp)
                _user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpireDays);

            await _userManager.UpdateAsync(_user);

            return new TokenDto ( AccessToken : accessToken, RefreshToken : refreshToken );
        }



        //public async Task<TokenDto> RefreshToken(TokenDto tokenDto)
        //{
        //    var principal = GetPrincipleFromExpiredToken(tokenDto.AccessToken);

        //    var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);

        //    if (string.IsNullOrEmpty(userId))
        //        throw new BadRequestException("User Id Claim is not exist in the token.");

        //    var user = await _userManager.FindByIdAsync(userId);

        //    if (user == null
        //        || !user.RefreshToken!.Equals(tokenDto.RefreshToken)
        //        || user.RefreshTokenExpiryTime <= DateTime.Now
        //        || user.IsRefreshTokenRevoked)
        //        throw new UnauthorizedException("Invalid,expired or revoked refresh token.");

        //    _user = user;

        //    return await GenerateTokens(false);
        //}


        //public async Task<bool> RevokeRefreshToken(string token, string id)
        //{
        //    var user = _userManager.Users
        //                .FirstOrDefault(user => user.RefreshToken!.Equals(token) && user.Id == id);

        //    if (user == null) return false;

        //    user.IsRefreshTokenRevoked = true;
        //    await _userManager.UpdateAsync(user);

        //    return true;
        //}

        public async Task<Result> ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) return Result.Success();

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            // Properly URL-encode both parameters
            var encodedEmail = Uri.EscapeDataString(email);
            var encodedTokenParam = Uri.EscapeDataString(encodedToken);

            var resetLink = $"{_frontendSettings.BaseUrl}/reset-password" +
                $"?email={encodedEmail}&token={encodedTokenParam}";

            //sending email to the user with the reset link that have the token
            await _emailService.SendPasswordResetEmailAsync(email, resetLink);

            _logger.LogInformation("Password reset link sent to {Email}", email);

            return Result.Success();
        }

        public async Task<Result> ResetPasswordAsync(ResetPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            //  return invalid token instead of user not found to avoid email enumeration.
            if (user == null) return Result.Failure(AuthErrors.InvalidToken);

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(dto.Token));

            var result = await _userManager.ResetPasswordAsync(user, decodedToken, dto.NewPassword);

            if (!result.Succeeded)
                return Result.Failure(AuthErrors.InvalidToken);

            _logger.LogInformation("Password reset successfully for {Email}", dto.Email);

            return Result.Success();

        }


        //public async Task<IdentityResult> ChangePasswordAsync(string userId, ChangePasswordDto dto)
        //{
        //    var user = await _userManager.FindByIdAsync(userId);

        //    if (user == null)
        //        return IdentityResult.Failed(new IdentityError { Description = "Unable to Change Password." });

        //    //change the password
        //    var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);

        //    return result;
        //}

        public async Task<bool> UserExists(Guid userId) =>
                await _userManager.FindByIdAsync(userId.ToString()) is not null;


        #region Helper

        private SigningCredentials GetSigningCredentials()
        {
            var key = _jwtSettings.Key;


            //Create security key
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            //Create signing credentials
            return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims()
        {
            //Create Claims
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,_user!.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name,_user.UserName ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var roles = await _userManager.GetRolesAsync(_user);

            //Add roles to claims
            var rolesClaims = roles.Select(role => new Claim("role", role));
            claims.AddRange(rolesClaims);

            return claims;
        }


        private string GetRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);

                return Convert.ToBase64String(randomNumber);
            }
        }

        //private ClaimsPrincipal GetPrincipleFromExpiredToken(string token)
        //{
        //    var tokenValidationParameters = new TokenValidationParameters
        //    {
        //        ValidateIssuer = true,
        //        ValidateAudience = true,
        //        ValidateLifetime = false,
        //        ValidateIssuerSigningKey = true,
        //        ValidIssuer = _jwtSettings.Issuer,
        //        ValidAudience = _jwtSettings.Audience,
        //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key))
        //    };

        //    var tokenHandler = new JwtSecurityTokenHandler();

        //    SecurityToken securityToken;
        //    var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

        //    var jwtSecurityToken = securityToken as JwtSecurityToken;

        //    if (jwtSecurityToken == null
        //        || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256
        //                    , StringComparison.InvariantCultureIgnoreCase))
        //        throw new BadRequestException("Expired token invalid.");

        //    return principal;
        //}



        #endregion
    }
}
