using Ofgem.API.BUS.BusinessAccounts.Domain.Entities;
using Ofgem.API.BUS.BusinessAccounts.Domain.Request;
using System;

namespace Ofgem.API.BUS.BusinessAccounts.Api.UnitTests
{
    public abstract class BaseServiceTests
    {

        protected static BusinessAccount CreateBusinessAccount()
        {
            return new BusinessAccount();
        }

        protected static PostBusinessAccountRequest CreateBusinessAccountRequest()
        {
            var postBusinessAccountRequest = new PostBusinessAccountRequest
            {
                DateAccountReceived = DateTime.Now,
                MCSCertificationNumber = "AAA-11111",
                CreatedBy = "TestInstaller@ofgem.gov.uk",
                BusinessName = "Test Installer",
                TradingName = "Test Installer Ltd",
                CompanyNumber = "12345678",

                BusinessAddress = new PostBusinessAccountRequestBusinessAddress
                {
                    AddressLine1 = "Test Adr Line 1",
                    Postcode = "TE11 1ST",
                    UPRN = "1234567890"
                }
            };
            return postBusinessAccountRequest;
        }

    }
}
