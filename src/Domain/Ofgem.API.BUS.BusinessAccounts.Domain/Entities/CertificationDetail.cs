namespace Ofgem.API.BUS.BusinessAccounts.Domain.Entities;

/// <summary>
/// Model for the CertificationDetail DB object
/// </summary>
public class CertificationDetail
{
    /// <summary>
    /// Object ID
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// FK to the Business Account
    /// </summary>
    public Guid BusinessAccountID { get; set; }
    /// <summary>
    /// FK to the Tech Type of the certification
    /// </summary>
    public Guid TechTypeCertificationID { get; set; }
    /// <summary>
    /// Start date of the Installer's Certification
    /// </summary>
    public DateTime StartDate { get; set; }
    /// <summary>
    /// Expiry date of the Installer's Certification
    /// </summary>
    public DateTime ExpiryDate { get; set; }
}
