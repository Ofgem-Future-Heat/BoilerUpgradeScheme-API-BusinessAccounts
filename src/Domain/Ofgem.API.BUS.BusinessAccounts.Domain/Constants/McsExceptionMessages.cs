namespace Ofgem.API.BUS.BusinessAccounts.Domain.Constants;

/// <summary>
/// This class defines exception messages related to the Mcs Service.
/// </summary>
public static class McsExceptionMessages
{
    /// <summary>
    /// Exception to describe that a Mcs Parameter is empty.
    /// </summary>
    public const string McsNumberFieldIsNullMessage = "MCS Number Field is null.";

    /// <summary>
    /// Exception to describe that a given Mcs number already exists.
    /// </summary>
    public const string McsNumberExistsMessageInBusinessAccount = "A business account with this MCS number has already been created.";

    /// <summary>
    /// Exception to describe that a given Mcs doesn't exist in the Mcs DB.
    /// </summary>
    public const string McsNumberDoesntExistInMcs = "Mcs Number doesnt exists in MCS's database.";
}