namespace Ofgem.API.BUS.BusinessAccounts.Domain.Constants;

/// <summary>
/// Class containing error messages for use in the business accounts controller & service.
/// </summary>
public static class AccountsExceptionMessages
{
    /// <summary>
    /// Error for when no guid is provided when checking if account exists
    /// </summary>
    public const string NoGuidError = "GUID provided is null.";

    /// <summary>
    /// Error for when an account with that GUID already exists
    /// </summary>
    public const string AccountExistsError = "The account already exists.";

    /// <summary>
    /// Error for when an account with the MCS number already exists in a valid state
    /// </summary>
    public const string McsNumberInUseError = "A business account with this MCS number has already been created";

    /// <summary>
    /// Error for no business account returned from the DB
    /// </summary>
    public const string NoAccountFound = "No account was found.";

    /// <summary>
    /// Error for no business account returned from the DB - provided account number not valid.
    /// </summary>
    public const string NoAccountProvided = "The account number provided was not a valid number.";

    /// <summary>
    /// Error for when no user associated with the business account is configured as authorised representative
    /// </summary>
    public const string NoAuthorisedRepresentativeFound = "The account has no authorised representative.";

    /// <summary>
    /// Error for when the LastUpdatedDate is defferent between the edited model and the one in the database
    /// </summary>
    public const string UpdatedSinceCollected = "The account was updated since the model was collected for editing.";

    public const string EmailAddressInUse = "This email is already in use on [businessName]'s installer account";
}