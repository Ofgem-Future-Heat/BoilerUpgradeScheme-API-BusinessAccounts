namespace Ofgem.API.BUS.BusinessAccounts.Domain.Entities;

/// <summary>
/// Model for the UserRole reference data DB object
/// </summary>
public class UserRole
{
    public Guid Id { get; set; }
    public string Description { get; set; } = null!;
}
