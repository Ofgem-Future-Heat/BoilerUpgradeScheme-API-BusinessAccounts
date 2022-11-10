using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Ofgem.API.BUS.BusinessAccounts.Domain.Exceptions;

/// <summary>
/// NotFoundException, thrown when item is not found
/// </summary>
/// <seealso cref="System.Exception" />
[Serializable]
public class ResourceNotFoundException : BadRequestException
{
    public ResourceNotFoundException(string message) : base(message, System.Net.HttpStatusCode.NoContent) { }

    public ResourceNotFoundException(string message, Dictionary<string, string[]> errors) : base(message, errors, System.Net.HttpStatusCode.NoContent) { }

    [ExcludeFromCodeCoverage]
    protected ResourceNotFoundException() { }

    [ExcludeFromCodeCoverage]
    protected ResourceNotFoundException(SerializationInfo info, StreamingContext context) :
        base(info, context)
    { }
}