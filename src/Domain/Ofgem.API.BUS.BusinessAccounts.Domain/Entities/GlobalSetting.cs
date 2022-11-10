namespace Ofgem.API.BUS.BusinessAccounts.Domain.Entities;

/// <summary>
/// Model for GlobalSetting DB object
/// </summary>
public class GlobalSetting
{
    /// <summary>
    /// Key - DB Generated
    /// </summary>
    public Guid ID { get; set; }

    /// <summary>
    /// DB Incremented - the Application Reference to be used by the process which made the row
    /// </summary>
    public int NextBusinessAccountReferenceNumber { get; set; }

    /// <summary>
    /// Requestor-generated Guid - allows requestor to find its own row
    /// </summary>
    public Guid GeneratedByID { get; set; }
}

