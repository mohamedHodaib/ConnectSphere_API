using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Common.DTOs
{
    public record RegisterDto(
        string UserName,
        string Email,
        string Password,
        string ConfirmPassword
    );

    public record LoginDto(
        string Email,
        string Password
    );


    public record ChangePasswordDto(
        string CurrentPassword,
        string NewPassword,
        string ConfirmNewPassword
    );


    public record ResetPasswordDto(
        string Email,
        string Token,
        string NewPassword,
        string ConfirmNewPassword
    );


    public record ForgotPasswordDto(
        string Email
    );


    public record TokenDto(
        string AccessToken,
        string RefreshToken
    );


    public record LogoutDto(
        string RefreshToken
    );

}
