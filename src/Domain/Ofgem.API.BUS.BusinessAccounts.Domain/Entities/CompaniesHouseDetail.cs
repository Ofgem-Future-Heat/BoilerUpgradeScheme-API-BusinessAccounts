using Ofgem.API.BUS.BusinessAccounts.Domain.Interfaces;

namespace Ofgem.API.BUS.BusinessAccounts.Domain.Entities;

/// <summary>
/// Model for the CompaniesHouseDetail DB object
/// </summary>
public class CompaniesHouseDetail : ICreateModify
{
    /// <summary>
    /// The date and time this record was created in the DB
    /// </summary>
    public DateTime CreatedDate { get; set; }
    /// <summary>
    /// The logon name (email address) of the user who created the object
    /// </summary>
    public string CreatedBy { get; set; }
    /// <summary>
    /// The date and time this record was last modified
    /// </summary>
    public DateTime? LastUpdatedDate { get; set; }
    /// <summary>
    /// The logon name (email address) of the user who most recently updated the object
    /// </summary>
    public string? LastUpdatedBy { get; set; }
    /// <summary>
    /// Unique identifier for the companies house details
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// The name of the company.
    /// </summary>
    public string? CompanyName { get; set; }
    /// <summary>
    /// The company number assigned to the company.
    /// </summary>
    public string? CompanyNumber { get; set; }
    /// <summary>
    /// The status of the company
    /// </summary>
    public string? CompanyStatus { get; set; }
    /// <summary>
    /// The trading name of the company.
    /// </summary>
    public string? TradingName { get; set; }
    /// <summary>
    /// The registered office of the company.
    /// </summary>
    public string? RegisteredOffice { get; set; }
    /// <summary>
    /// The company registration number.
    /// </summary>
    public string? CompanyRegistrationNumber { get; set; }
    /// <summary>
    /// the name of the parent company.
    /// </summary>
    public string? ParentCompanyName { get; set; }
    /// <summary>
    /// the company number of the parent company.
    /// </summary>
    public string? ParentCompanyNumber { get; set; }
    /// <summary>
    /// the unique identifier of the companies house address
    /// </summary>
    public Guid? RegisteredAddressId { get; set; }
    /// <summary>
    /// The telephone number of the company
    /// </summary>
    public string? TelephoneNumber { get; set; }
    /// <summary>
    /// The email address of the company
    /// </summary>
    public string? EmailAddress { get; set; }

    /// <summary>
    /// The ID of the business account
    /// </summary>
    public Guid BusinessAccountID { get; set; }
    public virtual BusinessAccount BusinessAccount { get; set; }
}