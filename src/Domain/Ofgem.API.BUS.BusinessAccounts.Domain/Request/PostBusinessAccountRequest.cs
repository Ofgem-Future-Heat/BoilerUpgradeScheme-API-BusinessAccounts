using System.ComponentModel.DataAnnotations;
using Ofgem.API.BUS.BusinessAccounts.Domain.Entities;

namespace Ofgem.API.BUS.BusinessAccounts.Domain.Request;

/// <summary>
/// The model for the POST request for adding business accounts
/// </summary>
public class PostBusinessAccountRequest
{
    /// <summary>
    /// The logon name (email address) of the user who created the request
    /// </summary>
    public string CreatedBy { get; set; } = null!;
    /// <summary>
    /// the name of the business
    /// </summary>
    [Required(ErrorMessage = "You must enter a business name.")]
    public string? BusinessName { get; set; }
    /// <summary>
    /// The trading name of the business
    /// </summary>
    public string? TradingName { get; set; }
    /// <summary>
    /// the MCS Certification number of the business
    /// </summary>
    [MaxLength(7)]
    [Required(ErrorMessage = "You must enter the business’s MCS certification number.")]
    public string MCSCertificationNumber { get; set; }
    /// <summary>
    /// The company number of the business
    /// </summary>
    [Required(ErrorMessage = "You must enter a company number.")]
    public string? CompanyNumber { get; set; }
    /// <summary>
    /// Boolean to confirm if installer is under investigation
    /// </summary>
    public bool IsUnderInvestigation { get; set; }
    /// <summary>
    /// the businesses Address
    /// </summary>
    [Required(ErrorMessage = "You must enter a business address.")]
    public PostBusinessAccountRequestBusinessAddress BusinessAddress { get; set; }
    /// <summary>
    /// the optional trading address
    /// </summary>
    public PostBusinessAccountRequestBusinessAddress? TradingAddress { get; set; }
    /// <summary>
    /// the date the account application was received
    /// </summary>
    public DateTime DateAccountReceived { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Partial bank account details for the business
    /// </summary>
    public PostBusinessAccountRequestBankAccount? BankAccount { get; set; }

    public ExternalUserAccount AuthorisedRepresentative { get; set; }
}