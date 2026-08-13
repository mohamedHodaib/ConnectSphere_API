using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Common.Interfaces.Services
{
    public interface IAuthService
    {
        Task<Result<UserProfileDetailsDto>> RegisterAsync(RegisterDto registerDto);
        Task<Result> ValidateAsync(LoginDto dto);
        Task<TokenDto> GenerateTokens(bool populateExp);
        Task<bool> UserExists(Guid userId);
        Task<Result> ConfirmEmailAsync(string userId, string token);
        //Task<TokenDto> RefreshToken(TokenDto tokenDto);
        //Task<IdentityResult> ChangePasswordAsync(string userId, ChangePasswordDto dto);
        Task<Result> ForgotPasswordAsync(string email);
        Task<Result> ResetPasswordAsync(ResetPasswordDto dto);
        //Task<bool> RevokeRefreshToken(string token, string userId);
    }
}
