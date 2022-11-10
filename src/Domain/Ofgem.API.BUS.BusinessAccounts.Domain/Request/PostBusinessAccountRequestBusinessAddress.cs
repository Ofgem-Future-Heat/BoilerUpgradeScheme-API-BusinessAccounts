using System.ComponentModel.DataAnnotations;

namespace Ofgem.API.BUS.BusinessAccounts.Domain.Request;

/// <summary>
/// The model for the BusinessAddress within the POST request for adding business accounts
/// </summary>
public class PostBusinessAccountRequestBusinessAddress
{
    /// <summary>
    /// The Unique Property Reference Number of the property being defined.
    /// </summary>
    public string? UPRN { get; set; }
    /// <summary>
    /// The first line of the address.
    /// </summary>
    [Required(ErrorMessage = "You must enter an address.")]
    public string AddressLine1 { get; set; }
    /// <summary>
    /// The second line of the address.
    /// </summary>
    public string? AddressLine2 { get; set; }
    /// <summary>
    /// The third line of the address.
    /// </summary>
    public string? AddressLine3 { get; set; }
    /// <summary>
    /// the fourth line of the address
    /// </summary>
    public string? AddressLine4 { get; set; }
    /// <summary>
    /// The county of which the property resides.
    /// </summary>
    public string? County { get; set; }
    /// <summary>
    /// The property's postcode.
    /// </summary>
    [Required(ErrorMessage = "You must enter a postcode.")]
    public string Postcode { get; set; }
}
