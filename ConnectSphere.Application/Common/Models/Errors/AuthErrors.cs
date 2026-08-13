using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Common.Models.Errors
{
    public static class AuthErrors
    {
        public static Error EmailConflict => Error.Conflict(
            "User.EmailConflict",
            "An account with this Email Address Already Exists.");

        public static Error UsernameConflict => Error.Conflict(
            "User.UsernameConflict",
            "An account with this Username Already Exists.");


        public static Error WrongCredentials => Error.Unauthorized(
            "User.LoginFailed",
            "Wrong Credentials.");

        public static Error Lockedout => Error.Unauthorized(
            "User.Lockout",
            "User account is locked out.");

        public static Error UserNotFound => Error.NotFound(
            "User.NotFound",
            "User is not found");


        public static  Error EmailNotConfirmed =>
            Error.Unauthorized("Auth.EmailNotConfirmed", "Please confirm your email before logging in.");
        public static  Error InvalidToken =>
            Error.Validation("Auth.InvalidToken", "The confirmation token is invalid or has expired.");

        public static  Error EmailAlreadyConfirmed =>
            Error.Conflict("Auth.EmailAlreadyConfirmed", "This email has already been confirmed.");
    }
}
