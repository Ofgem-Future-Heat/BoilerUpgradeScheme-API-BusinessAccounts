using Ofgem.API.BUS.BusinessAccounts.Domain.Interfaces;

namespace Ofgem.API.BUS.BusinessAccounts.Domain.Entities;

/// <summary>
/// The Business Account of the installers.
/// </summary>
public class BusinessAccount : ICreateModify
{
    /// <summary>
    /// Id of business account (installer).
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// unique business account number
    /// </summary>
    public string BusinessAccountNumber { get; set; }
    /// <summary>
    /// The date and time this record was created in the DB
    /// </summary>
    public DateTime CreatedDate { get; set; }
    /// <summary>
    /// The logon name (email address) of the user who created the business account
    /// </summary>
    public string CreatedBy { get; set; }
    /// <summary>
    /// The date and time this record was last modified
    /// </summary>
    public DateTime? LastUpdatedDate { get; set; }
    /// <summary>
    /// The logon name (email address) of the user who most recently updated the business account
    /// </summary>
    public string? LastUpdatedBy { get; set; }
    /// <summary>
    /// date the account setup was requested
    /// </summary>
    public DateTime AccountSetupRequestDate { get; set; }
    /// <summary>
    /// The date the account is made active
    /// </summary>
    public DateTime? ActiveDate { get; set; }
    /// <summary>
    /// Business Name of business account (installer).
    /// </summary>
    public string? BusinessName { get; set; }
    /// <summary>
    /// A business accounts Business trading name.
    /// </summary>
    public string? TradingName { get; set; }
    /// <summary>
    /// Id of a Substatus.
    /// </summary>
    public Guid SubStatusId { get; set; }
    /// <summary>
    /// Id of a company type.
    /// </summary>
    public Guid? CompanyTypeId { get; set; }
    /// <summary>
    /// The MCS certification number for the business link to the account.
    /// </summary>
    public string MCSCertificationNumber { get; set; }
    /// <summary>
    /// The governing Body that approved the business' certification.
    /// </summary>
    public string? MCSCertificationBody { get; set; }
    /// <summary>
    /// The MCS membership number.
    /// </summary>
    public string? MCSMembershipNumber { get; set; }
    /// <summary>
    /// The MCS consumer code related to the business.
    /// </summary>
    public string? MCSConsumerCode { get; set; }
    /// <summary>
    /// The company type as stated by MCS.
    /// </summary>
    public string? MCSCompanyType { get; set; }
    /// <summary>
    /// The MCS Id.
    /// </summary>
    public string? MCSId { get; set; }
    /// <summary>
    /// Guid for MCS Contact details ID
    /// </summary>
    public Guid? MCSContactDetailsID { get; set; }
    /// <summary>
    /// The MCS Address Id for the address associated with the related business account.
    /// </summary>
    public Guid? MCSAddressID { get; set; }
    /// <summary>
    /// Guid for the companies house Id for the business
    /// </summary>
    public Guid? CoHoID { get; set; }
    /// <summary>
    /// Identifies when a Business Account is under investigation
    /// </summary>
    public bool IsUnderInvestigation { get; set; }
    /// <summary>
    /// Used to record the Business Account recommended decision (Pass or Fail) for the QC reviewer
    /// </summary>
    public bool? QCRecommendation { get; set; }
    /// <summary>
    /// Used to record the Business Account recommended decision (Pass or Fail) for the DA reviewer
    /// </summary>
    public bool? DARecommendation {get; set; }
    public virtual CompanyType? CompanyType { get; set; }
    public virtual List<BankAccount>? BankAccounts { get; set; }
    public virtual List<BusinessAddress>? BusinessAddresses { get; set; }
    public virtual BusinessAccountSubStatus? SubStatus { get; set; }
    public virtual CompaniesHouseDetail? CompaniesHouseDetails { get; set; }
}