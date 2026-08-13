using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Common.Models
{
    //Generic
    public class Result<T> : IResult
    {
        public bool IsSuccess { get; }
        public T? Data { get; }

        // Use IReadOnlyList for immutability
        public IReadOnlyList<Error> Errors { get; }

        // Private constructor to force use of factory methods
        private Result(bool isSuccess, T? data, IReadOnlyList<Error>? errors)
        {
            IsSuccess = isSuccess;
            Data = data;
            Errors = errors ?? Array.Empty<Error>();
        }

        /// <summary>
        /// Creates a successful result with data.
        /// </summary>
        public static Result<T> Success(T data) =>
            new(true, data, null);

        /// <summary>
        /// Creates a failure result with a single error.
        /// </summary>
        public static Result<T> Failure(Error error) =>
            new(false, default, new[] { error });

        /// <summary>
        /// Creates a failure result with multiple errors.
        /// </summary>
        public static Result<T> Failure(IReadOnlyList<Error> errors) =>
            new(false, default, errors);
    }


    //Non Generic
    public class Result
    {
        public bool IsSuccess { get; }
        public IReadOnlyList<Error> Errors { get; }

        private Result(bool isSuccess, IReadOnlyList<Error>? errors)
        {
            IsSuccess = isSuccess;
            Errors = errors ?? Array.Empty<Error>();
        }

        /// <summary>
        /// Creates a successful result.
        /// </summary>
        public static Result Success() =>
            new(true, null);

        /// <summary>
        /// Creates a failure result with a single error.
        /// </summary>
        public static Result Failure(Error error) =>
            new(false, new[] { error });

        /// <summary>
        /// Creates a failure result with multiple errors.
        /// </summary>
        public static Result Failure(IReadOnlyList<Error> errors) =>
            new(false, errors);
    }

}
