namespace Ofgem.API.BUS.BusinessAccounts.Domain.Request;

/// <summary>
/// the model for the POST request to create new users
/// </summary>
public class PostUserAccountRequest
{
    /// <summary>
    /// the business account ID to link to a business account
    /// </summary>
    public Guid BusinessAccountID { get; set; }
    /// <summary>
    /// The email address of the creating user
    /// </summary>
    public string CreatedBy { get; set; }
    /// <summary>
    /// List of all users that need adding
    /// </summary>
    public List<PostUserAccountRequestExternalUserAccount> ExternalUserAccounts { get; set; } = new();
}