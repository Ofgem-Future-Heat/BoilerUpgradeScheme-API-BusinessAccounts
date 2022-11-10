using System.Collections.Immutable;
using static Ofgem.API.BUS.BusinessAccounts.Domain.Entities.AddressType;

namespace Ofgem.API.BUS.BusinessAccounts.Domain.Constants;

/// <summary>
/// Constants class for Type refdata
/// </summary>
public static class TypeMappings
{
    public class TypeFields
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
    }

    public static readonly ImmutableDictionary<AddressTypeCode, TypeFields> AddressType =
        ImmutableDictionary<AddressTypeCode, TypeFields>.Empty
        .Add(AddressTypeCode.BIZ, new TypeFields
        {
            Id = new Guid("2B7C0FBE-81FB-48AF-B3D4-99B6941C3CB0"),
            DisplayName = "Business Address"
        })
        .Add(AddressTypeCode.TRADE, new TypeFields
        {
            Id = new Guid("454ACCB1-3D67-4FCC-A92E-DCB07384CA14"),
            DisplayName = "Trading Address"
        });
}
