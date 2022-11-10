using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Runtime.Serialization;

namespace Ofgem.API.BUS.BusinessAccounts.Domain.Exceptions;

/// <summary>
/// BadRequestException, thrown when service is invalid
/// </summary>
/// <seealso cref="System.Exception" />
[Serializable]
public class BadRequestException : Exception
{
    /// <summary>
    /// Gets or sets the status code.
    /// </summary>
    /// <value>
    /// The status code.
    /// </value>
    public HttpStatusCode StatusCode { get; protected set; } = HttpStatusCode.BadRequest;

    private Dictionary<string, string[]> _errors;

    public Dictionary<string, string[]> Errors => _errors;

    public BadRequestException(string message) : base(message) { }

    public BadRequestException(string message, HttpStatusCode statusCode) : this(message)
    {
        StatusCode = statusCode;
    }

    public BadRequestException(string message, Dictionary<string, string[]> errors) : base(message)
    {
        _errors = errors;
    }

    public BadRequestException(string message, Dictionary<string, string[]> errors, HttpStatusCode statusCode) : this(message, statusCode)
    {
        _errors = errors;
    }

    [ExcludeFromCodeCoverage]
    protected BadRequestException() { }

    [ExcludeFromCodeCoverage]
    protected BadRequestException(SerializationInfo info, StreamingContext context) :
        base(info, context)
    { }
}