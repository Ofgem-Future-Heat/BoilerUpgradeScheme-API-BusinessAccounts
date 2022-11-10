namespace Ofgem.API.BUS.BusinessAccounts.Domain.Entities;

/// <summary>
/// Model for the BusinessAccountStatusHistory DB object
/// </summary>
public class BusinessAccountStatusHistory
{
    /// <summary>
    /// Object ID
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// The ID of the business account
    /// </summary>
    public Guid BusinessAccountID { get; set; }
    /// <summary>
    /// the BusinessAccountSubStatus ID
    /// </summary>
    public Guid SubStatusId { get; set; }
    /// <summary>
    /// The date and time this status was applied
    /// </summary>
    public DateTime StartDateTime { get; set; }
    /// <summary>
    /// The date and time this status was superseded, if applicable
    /// </summary>
    public DateTime? EndDateTime { get; set; }
}