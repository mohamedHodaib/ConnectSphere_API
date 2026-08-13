namespace ConnectSphere.Application.Common.Models;

/// <summary>
/// Represents a structured error, distinguishing between different failure types.
/// </summary>
public class Error
{
    public string Code { get; }
    public string Description { get; }

    /// <summary>
    /// A numeric or enum-like representation of the error type.
    /// </summary>
    public ErrorType Type { get; }

    private Error(string code, string description, ErrorType type)
    {
        Code = code;
        Description = description;
        Type = type;
    }

    // --- Factory Methods for Error Types ---

    /// <summary>
    /// Creates a 'Not Found' error.
    /// </summary>
    public static Error NotFound(string code, string description) =>
        new(code, description, ErrorType.NotFound);

    /// <summary>
    /// Creates a 'Validation' error.
    /// </summary>
    public static Error Validation(string code, string description) =>
        new(code, description, ErrorType.Validation);

    /// <summary>
    /// Creates a 'Conflict' error.
    /// </summary>
    public static Error Conflict(string code, string description) =>
        new(code, description, ErrorType.Conflict);

    /// <summary>
    /// Creates a 'Unauthorized' error.
    /// </summary>
    public static Error Unauthorized(string code, string description) =>
        new(code, description, ErrorType.Unauthorized);

    /// <summary>
    /// Creates a 'Forbidden' error.
    /// </summary>
    public static Error Forbidden(string code, string description) =>
        new(code, description, ErrorType.Forbidden);

    /// <summary>
    /// Creates a 'General' or 'Unexpected' error.
    /// </summary>
    public static Error General(string code, string description) =>
        new(code, description, ErrorType.General);
}

/// <summary>
/// Defines the high-level categories of errors.
/// </summary>
public enum ErrorType
{
    General,
    Validation,
    NotFound,
    Conflict,
    Unauthorized,
    Forbidden
}