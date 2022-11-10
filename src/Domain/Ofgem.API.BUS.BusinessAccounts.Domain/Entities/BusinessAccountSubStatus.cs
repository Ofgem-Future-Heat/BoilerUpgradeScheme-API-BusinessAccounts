namespace Ofgem.API.BUS.BusinessAccounts.Domain.Entities;

/// <summary>
/// Model for the BusinessAccountSubStatus reference data DB object
/// </summary>
public class BusinessAccountSubStatus
{
    public enum BusinessAccountSubStatusCode
    {
        WITHDR,
        DA,
        INREV,
        ACTIV,
        SUBMIT,
        QC,
        SUSPEND,
        WITHI,
        REVOK,
        FAIL
    }
    /// <summary>
    /// A sub statuses unique Id.
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// A status code.
    /// </summary>
    public BusinessAccountSubStatusCode Code { get; set; }
    /// <summary>
    /// A display name for the UI.
    /// </summary>
    public string DisplayName { get; set; }
    /// <summary>
    /// A brief description for the sub status.
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// A sort order for the dropdown display in the UI.
    /// </summary>
    public int SortOrder { get; set; }
    /// <summary>
    /// The unique Id of the parent status.
    /// </summary>
    public Guid BusinessAccountStatusId { get; set; }
    /// <summary>
    /// The parent status of the selected sub status.
    /// </summary>
    public virtual BusinessAccountStatus BusinessAccountStatus { get; set; }
}