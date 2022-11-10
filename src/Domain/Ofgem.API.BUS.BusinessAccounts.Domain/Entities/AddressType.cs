namespace Ofgem.API.BUS.BusinessAccounts.Domain.Entities;

/// <summary>
/// Model for the AddressType reference data DB object
/// </summary>
public class AddressType
{    public enum AddressTypeCode
    {
        BIZ,
        TRADE
    }

    /// <summary>
    /// The AddressType's unique Id.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// A brief description of the address type.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// A status code.
    /// </summary>
    public AddressTypeCode Code { get; set; }

    /// <summary>
    /// A display name for the UI.
    /// </summary>
    public string DisplayName { get; set; }

    /// <summary>
    /// A sort order for any dropdown display in the UI.
    /// </summary>
    public int SortOrder { get; set; }
}