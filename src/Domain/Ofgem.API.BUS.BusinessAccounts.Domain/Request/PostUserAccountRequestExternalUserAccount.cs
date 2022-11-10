namespace Ofgem.API.BUS.BusinessAccounts.Domain.Request;

public class PostUserAccountRequestExternalUserAccount
{
    /// <summary>
    /// The first name of the user
    /// </summary>
    public string FirstName { get; set; } = null!;

    /// <summary>
    /// The last name of the user
    /// </summary>
    public string LastName { get; set; } = null!;
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
}
