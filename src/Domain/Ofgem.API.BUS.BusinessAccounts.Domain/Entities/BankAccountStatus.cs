namespace Ofgem.API.BUS.BusinessAccounts.Domain.Entities;

/// <summary>
/// Model for the BankAccountStatus reference data DB object
/// </summary>
public class BankAccountStatus
{
    public enum BankAccountStatusCode
    {
        ACTIVE,
        CLOSED
    }
    /// <summary>
    /// The unique identifier of the bank account status
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// A status code.
    /// </summary>
    public BankAccountStatusCode Code { get; set; }
    /// <summary>
    /// A display name for the UI.
    /// </summary>
    public string DisplayName { get; set; }
    /// <summary>
    /// A brief description of the status.
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// A sort order for the dropdown display in the UI.
    /// </summary>
    public int SortOrder { get; set; }
}
