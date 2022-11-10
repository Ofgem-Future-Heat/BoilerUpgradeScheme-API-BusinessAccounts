namespace Ofgem.API.BUS.BusinessAccounts.Domain.Entities;

/// <summary>
/// Model for the CompanyType reference data DB object
/// </summary>
public class CompanyType
{
    /// <summary>
    /// A company types unique Id.
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// A brief description of the company type.
    /// </summary>
    public string Description { get; set; }
}