using Ofgem.API.BUS.BusinessAccounts.Domain.Interfaces;

namespace Ofgem.API.BUS.BusinessAccounts.Domain.Entities;

/// <summary>
/// Address Property
/// </summary>
public class BusinessAddress : ICreateModify
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
    /// The addresses unique Id.
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// The Unique Property Reference Number of the property being defined.
    /// </summary>
    public string? UPRN { get; set; }
    /// <summary>
    /// The first line of the address.
    /// </summary>
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
    public string Postcode { get; set; }    
    /// <summary>
    /// Id of the AddressType row for the entity's Type
    /// </summary>
    public Guid AddressTypeId { get; set; }
    /// <summary>
    /// Navigation property to the AddressType of this entity, e.g. business address, trading address
    /// </summary>
    public virtual AddressType? AddressType { get; set; }

    /// <summary>
    /// Business account ID for the address
    /// </summary>
    public virtual Guid BusinessAccountId { get; set; }
}