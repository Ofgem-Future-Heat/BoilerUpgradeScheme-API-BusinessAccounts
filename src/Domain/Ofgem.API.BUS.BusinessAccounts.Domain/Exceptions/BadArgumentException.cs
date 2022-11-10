using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Ofgem.API.BUS.BusinessAccounts.Domain.Exceptions;

/// <summary>
/// BadArgumentException, thrown when service arguments are invalid
/// </summary>
/// <seealso cref="System.Exception" />
[Serializable]
public class BadArgumentException : BadRequestException
{
    public BadArgumentException(string message) : base(message) { }

    public BadArgumentException(string message, Dictionary<string, string[]> errors) : base(message, errors) { }

    [ExcludeFromCodeCoverage]
    protected BadArgumentException() { }

    [ExcludeFromCodeCoverage]
    protected BadArgumentException(SerializationInfo info, StreamingContext context) :
        base(info, context)
    { }
}