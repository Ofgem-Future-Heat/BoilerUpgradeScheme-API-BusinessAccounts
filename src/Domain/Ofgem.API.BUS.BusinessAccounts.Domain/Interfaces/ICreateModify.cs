namespace Ofgem.API.BUS.BusinessAccounts.Domain.Interfaces;

/// <summary>
/// Interface marking a DB class as requiring 
/// 'By' (e.g. 'joe.bloggs@ofgem.gov.uk') and 'Date' values
/// for Creation and most recent update.
/// Implemented in each class, and Date portions handled by overridden 
/// SaveChangesAsync method in DbContext 
/// </summary>
public interface ICreateModify
{    
    /// <summary>
    /// Date and time the record was created
    /// </summary>
    public DateTime CreatedDate { get; set; }
    /// <summary>
    /// Email address of the user who created the record
    /// </summary>
    public string CreatedBy { get; set; }
    /// <summary>
    /// DateAndTime the record was last updated
    /// </summary>
    public DateTime? LastUpdatedDate { get; set; }
    /// <summary>
    /// Email address of the user who last updated the record
    /// </summary>
    public string? LastUpdatedBy { get; set; }
}
