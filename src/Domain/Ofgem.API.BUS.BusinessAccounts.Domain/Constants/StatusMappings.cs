using System.Collections.Immutable;
using static Ofgem.API.BUS.BusinessAccounts.Domain.Entities.BankAccountStatus;
using static Ofgem.API.BUS.BusinessAccounts.Domain.Entities.BusinessAccountSubStatus;
using static Ofgem.API.BUS.BusinessAccounts.Domain.Entities.InviteStatus;

namespace Ofgem.API.BUS.BusinessAccounts.Domain.Constants;

public static class StatusMappings
{
    public class StatusFields
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }

    }

    public static readonly ImmutableDictionary<BusinessAccountSubStatusCode, StatusFields> BusinessAccountSubStatus =
        ImmutableDictionary<BusinessAccountSubStatusCode, StatusFields>.Empty
        .Add(BusinessAccountSubStatusCode.ACTIV, new StatusFields { Id = new Guid("16671635-2BF2-4056-8FDA-227A6AE3B631"), DisplayName = "Active" })
        .Add(BusinessAccountSubStatusCode.DA, new StatusFields { Id = new Guid("93EEC01C-23B4-4D4B-BAD5-4CBFE61D1383"), DisplayName = "DA" })
        .Add(BusinessAccountSubStatusCode.FAIL, new StatusFields { Id = new Guid("C42A347D-B5F1-4BF4-924C-72ADCBFD56A1"), DisplayName = "Failed" })
        .Add(BusinessAccountSubStatusCode.INREV, new StatusFields { Id = new Guid("3585D921-9FB5-48DD-B50D-4999931998C3"), DisplayName = "In review" })
        .Add(BusinessAccountSubStatusCode.QC, new StatusFields { Id = new Guid("183E59A0-77AE-4AFE-8A62-C52402F39291"), DisplayName = "QC" })
        .Add(BusinessAccountSubStatusCode.REVOK, new StatusFields { Id = new Guid("1480C459-3414-44FC-8BDB-003A87F71CCB"), DisplayName = "Revoked" })
        .Add(BusinessAccountSubStatusCode.SUBMIT, new StatusFields { Id = new Guid("7D70D691-1B39-4AC3-8715-9FE1960AC8CA"), DisplayName = "Submitted" })
        .Add(BusinessAccountSubStatusCode.SUSPEND, new StatusFields { Id = new Guid("545B1D6B-2C68-4FC2-BB32-F85198D071BF"), DisplayName = "Suspended" })
        .Add(BusinessAccountSubStatusCode.WITHDR, new StatusFields { Id = new Guid("549E0D4D-694D-4BBB-B4F7-5AB8525A6E8D"), DisplayName = "Withdrawn" })
        .Add(BusinessAccountSubStatusCode.WITHI, new StatusFields { Id = new Guid("E65E8D68-3000-444B-B139-1AA03B274B9A"), DisplayName = "With installer" });


    public static readonly ImmutableDictionary<BankAccountStatusCode, StatusFields> BankAccountStatus =
        ImmutableDictionary<BankAccountStatusCode, StatusFields>.Empty
        .Add(BankAccountStatusCode.ACTIVE, new StatusFields { Id = new Guid("9C328895-0023-4776-A0EB-7AD73EE51CAA"), DisplayName = "Active" })
        .Add(BankAccountStatusCode.CLOSED, new StatusFields { Id = new Guid("B29108C9-82B2-4CA2-B471-17E8AA741EA1"), DisplayName = "Closed" });


    public class InviteStatusFields
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
    }

    public static readonly ImmutableDictionary<InviteStatusCode, InviteStatusFields> InviteStatus =
        ImmutableDictionary<InviteStatusCode, InviteStatusFields>.Empty
        .Add(InviteStatusCode.NOTSENT, new InviteStatusFields { Id = new Guid("1F705C16-74C3-4B46-B3DB-48C267BEBA49"), DisplayName = "Invite Not Sent", Description = "Invite Not Sent" })
        .Add(InviteStatusCode.INVITED, new InviteStatusFields { Id = new Guid("D8A3632D-2629-43CC-8BFD-B198DB6B97AB"), DisplayName = "Invited", Description = "Invited" })
        .Add(InviteStatusCode.NOTDELIVRD, new InviteStatusFields { Id = new Guid("D08080C0-EFA3-4E21-BE39-DAEC74A0BB56"), DisplayName = "Invite Not Delivered", Description = "Invite Not Delivered" })
        .Add(InviteStatusCode.EXPIRED, new InviteStatusFields { Id = new Guid("EB989258-B1FE-4733-885D-98F96E95B31C"), DisplayName = "Invite Expired", Description = "Invite Expired" })
        .Add(InviteStatusCode.SIGNEDUP, new InviteStatusFields { Id = new Guid("9FF91CE7-6A54-47CF-980B-A28687F0DDC0"), DisplayName = "Signed Up", Description = "Signed Up" })
        .Add(InviteStatusCode.CANCELLED, new InviteStatusFields { Id = new Guid("1A853F93-94CE-4AC5-938C-5A943E1FD0F7"), DisplayName = "Invite Cancelled", Description = "Invite Cancelled" });

}
