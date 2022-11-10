namespace Ofgem.API.BUS.BusinessAccounts.Domain.Request;

public class UpdateBusinessAccountStatusRequest
{
    public Guid BusinessAccountId { get; set; }
    public Guid StatusId { get; set; }
    public string RequestedByUser { get; set; }
}
