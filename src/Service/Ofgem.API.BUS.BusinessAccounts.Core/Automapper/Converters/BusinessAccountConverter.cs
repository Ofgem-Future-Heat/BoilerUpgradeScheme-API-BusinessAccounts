using AutoMapper;
using Ofgem.API.BUS.BusinessAccounts.Domain.Constants;
using Ofgem.API.BUS.BusinessAccounts.Domain.Entities;
using Ofgem.API.BUS.BusinessAccounts.Domain.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ofgem.API.BUS.BusinessAccounts.Core.Automapper.Converters;

public class BusinessAccountConverter : ITypeConverter<PostBusinessAccountRequest, BusinessAccount>
{
    /// <summary>
    /// Converter to convert the post business account request to the business account type
    /// </summary>
    /// <param name="source">Source type</param>
    /// <param name="destination">Destination Type</param>
    /// <param name="context"></param>
    /// <returns></returns>
    public BusinessAccount Convert(PostBusinessAccountRequest source, BusinessAccount destination, ResolutionContext context)
    {
        var businessAccount = new BusinessAccount()
        {
            CreatedBy = source.CreatedBy,
            BusinessName = source.BusinessName,
            IsUnderInvestigation = source.IsUnderInvestigation,
            TradingName = source.TradingName,
            MCSCertificationNumber = source.MCSCertificationNumber,
            CompaniesHouseDetails = new CompaniesHouseDetail()
            {
                CreatedBy = source.CreatedBy,
                CreatedDate = DateTime.Now,
                CompanyName = source.BusinessName,
                CompanyNumber = source.CompanyNumber,
                Id = Guid.NewGuid(),
            },
            BusinessAddresses = new List<BusinessAddress>() 
            { 
                new BusinessAddress()
                {
                    CreatedBy = source.CreatedBy,
                    UPRN = source.BusinessAddress.UPRN,
                    AddressLine1 = source.BusinessAddress.AddressLine1,
                    AddressLine2 = source.BusinessAddress.AddressLine2,
                    AddressLine3 = source.BusinessAddress.AddressLine3,
                    AddressLine4 = source.BusinessAddress.AddressLine4,
                    County = source.BusinessAddress.County,
                    Postcode = source.BusinessAddress.Postcode,
                    AddressTypeId = TypeMappings.AddressType[AddressType.AddressTypeCode.BIZ].Id
                }
            },
            AccountSetupRequestDate = source.DateAccountReceived
        };

        if (source.BankAccount is not null)
        {
            businessAccount.BankAccounts = new List<BankAccount>()
            { 
                new BankAccount()
                {
                    CreatedBy = source.CreatedBy,
                    SortCode = source.BankAccount.SortCode,
                    AccountName = source.BankAccount.AccountName,
                    AccountNumber = source.BankAccount.AccountNumber
                }
            };
        }

        if (source.TradingAddress is not null)
        {
            businessAccount.BusinessAddresses.Add(new BusinessAddress()
            {
                CreatedBy = source.CreatedBy,
                AddressTypeId = TypeMappings.AddressType[AddressType.AddressTypeCode.TRADE].Id,
                Postcode = source.TradingAddress.Postcode,
                AddressLine1 = source.TradingAddress.AddressLine1,
                AddressLine2 = source.TradingAddress.AddressLine2,
                AddressLine3 = source.TradingAddress.AddressLine3,
                AddressLine4 = source.TradingAddress.AddressLine4,
                County = source.TradingAddress.County,
                UPRN = source.TradingAddress.UPRN

            });
        }

        return businessAccount;
    }
}
