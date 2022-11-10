using Ofgem.API.BUS.BusinessAccounts.Domain.Interfaces;

namespace Ofgem.API.BUS.BusinessAccounts.Domain.Entities;

/// <summary>
/// The Bank Account of installer.
/// </summary>
public class BankAccount : ICreateModify
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
    /// The unique identifier for the bank account selected.
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// The name attributed to the selected bank account.
    /// </summary>
    public string AccountName { get; set; }
    /// <summary>
    /// The last two digits of the bank account's sort code.
    /// </summary>
    public string SortCode { get; set; }
    /// <summary>
    /// The last four digits of the bank account's account number.
    /// </summary>
    public string AccountNumber { get; set; }
    /// <summary>
    /// Account number for the SUN payments system
    /// </summary>
    public string? SunAccountNumber { get; set; }
    /// <summary>
    /// foreign key to business account
    /// </summary>
    public Guid BusinessAccountID { get; set; }
    /// <summary>
    /// ID of the current status
    /// </summary>
    public Guid? StatusID { get; set; }
    /// <summary>
    /// Navigation property to the Bank account status
    /// </summary>
    public virtual BankAccountStatus? Status { get; set; }
}