using Ofgem.API.BUS.BusinessAccounts.Domain.Interfaces;

namespace Ofgem.API.BUS.BusinessAccounts.Domain.Entities;

/// <summary>
/// the Model for external user account setup
/// </summary>
public class ExternalUserAccount : ICreateModify
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
    /// Unique identifier of the external user
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// the unique identifier of the business linked to this user
    /// </summary>
    public Guid BusinessAccountID { get; set; }
    /// <summary>
    /// the full name of the user 
    /// </summary>
    public string? FullName { get; set; }
    /// <summary>
    /// the role id of the user
    /// </summary>
    public Guid? RoleId { get; set; }
    /// <summary>
    /// the telephone number of the user
    /// </summary>
    public string? TelephoneNumber { get; set; }
    /// <summary>
    /// the email address of the user
    /// </summary>
    public string EmailAddress { get; set; }
    /// <summary>
    /// the UPRN of the home address
    /// </summary>
    public string? HomeAddressUPRN { get; set; }
    /// <summary>
    /// the users home address
    /// </summary>
    public string? HomeAddress { get; set; }
    /// <summary>
    /// he user's home address postcode
    /// </summary>
    public string? HomeAddressPostcode { get; set; }
    /// <summary>
    /// the companies house role id of the user
    /// </summary>
    public Guid? CoHoRoleID { get; set; }
    /// <summary>
    /// Flag to indicate that the user is a SuperUser
    /// </summary>
    public bool SuperUser { get; set; } = false;
    /// <summary>
    /// Flag to indicate that the user is a Standard User
    /// </summary>
    public bool StandardUser { get; set; } = true;
    /// <summary>
    /// the AD B2C Login ID for the user
    /// </summary>
    public Guid? AADB2CId { get; set; }
    /// <summary>
    /// The user's date of birth
    /// </summary>
    public DateTime? DOB { get; set; }
    /// <summary>
    /// A flag to identify the authorisedRepresentative for the Installer business
    /// </summary>
    public bool AuthorisedRepresentative { get; set; } = false;

    /// <summary>
    /// A flag to identify whether the user is obsolete, e.g. upon user leaving the installer business
    /// </summary>
    public bool IsObsolete { get; set; } = false;

    /// <summary>
    /// The first name of the user
    /// </summary>
    public string FirstName { get; set; } = null!;

    /// <summary>
    /// The last name of the user
    /// </summary>
    public string LastName { get; set; } = null!;

    /// <summary>
    /// Version number of the terms and conditions wording most recently accepted by the user
    /// </summary>
    public int? TermsLastAcceptedVersion { get; set; }

    /// <summary>
    /// DateTime when the terms and conditions were last accepted by the user
    /// </summary>
    public DateTime? TermsLastAcceptedDate { get; set; }
}