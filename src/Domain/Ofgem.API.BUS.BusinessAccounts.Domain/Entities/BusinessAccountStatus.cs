namespace Ofgem.API.BUS.BusinessAccounts.Domain.Entities;

/// <summary>
/// Model for the BusinessAccountStatus reference data DB object
/// </summary>
public class BusinessAccountStatus
{
    public enum BusinessAccountStatusCode
    {
        WITHDR,
        IN_REV,
        ACTIVE,
        SUBMIT,
        SUSPEN,
        WITHINS,
        REVOKE,
        FAILED
    }
    /// <summary>
    /// A status' unique Id
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// A display name for the UI.
    /// </summary>
    public string DisplayName { get; set; }
    /// <summary>
    /// A status code.
    /// </summary>
    public BusinessAccountStatusCode Code { get; set; }
    /// <summary>
    /// A brief description of the status.
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// A sort order for the dropdown display in the UI.
    /// </summary>
    public int SortOrder { get; set; }
    /// <summary>
    /// A list of Sub statuses (Child Statuses to a parent Status).
    /// </summary>
    public virtual List<BusinessAccountSubStatus>? BusinessAccountSubStatuses { get; set; }

}