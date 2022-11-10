namespace Ofgem.API.BUS.BusinessAccounts.Domain.Entities;

/// <summary>
/// Model for the BankAccountStatus reference data DB object
/// </summary>
public class InviteStatus
{
    public enum InviteStatusCode
    {
        NOTSENT,
        INVITED,
        NOTDELIVRD,
        EXPIRED,
        SIGNEDUP,
        CANCELLED
    }

    /// <summary>
    /// The unique identifier of the invite status
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// A status code.
    /// </summary>
    public InviteStatusCode Code { get; set; }
    /// <summary>
    /// A display name for the UI.
    /// </summary>
    public string DisplayName { get; set; }
    /// <summary>
    /// A brief description of the status.
    /// </summary>
    public string Description { get; set; }
}
