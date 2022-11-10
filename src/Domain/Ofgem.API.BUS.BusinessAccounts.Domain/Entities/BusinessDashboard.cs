using Microsoft.EntityFrameworkCore;

namespace Ofgem.API.BUS.BusinessAccounts.Domain.Entities;

[Keyless]
public class BusinessDashboard
{
    public string? BusinessID { get; set; }
    public string? BusinessName { get; set; }

    public string? BusinessAccountNumber { get; set; }

    public string? BusinessSubStatus { get; set; }

    public string? BusinessSubStatusCode { get; set; }

    public string? ReviewRecommendation { get; set; }

    public string? AccountSetupRequestDate { get; set; }

    public string? BeingAmended { get; set; }
}
