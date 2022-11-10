using AutoMapper;
using Moq;
using NUnit.Framework;
using Ofgem.API.BUS.BusinessAccounts.Core;
using Ofgem.API.BUS.BusinessAccounts.Core.Interfaces;
using Ofgem.API.BUS.BusinessAccounts.Domain.Entities;
using Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess.Interfaces;
using System;
using System.Collections.Generic;

namespace Ofgem.API.BUS.BusinessAccounts.Api.UnitTests;
public abstract class BusinessAccountBaseTests : BaseServiceTests
{
    protected Mock<IMapper> _mockMapper = new();
    protected Mock<IAccountsService> _mockAccountsService = new();
    protected Mock<IMcsService> _mockMcsService = new();
    protected Mock<IBusinessAccountProvider> _mockBusinessAccountsProvider = new();
    protected Mock<IInviteService> _mockInviteService = new();

    [SetUp]
    public void Setup()
    {
        _mockMapper = new();
        _mockAccountsService = new();
        _mockMcsService = new();
        _mockBusinessAccountsProvider = new();
        _mockInviteService = new();
    }

    public static readonly Guid BusinessAccountId = Guid.NewGuid();
    public static readonly string BusinessAccountName = "Test Business";
    public static readonly string BusinessAccountNumber = "12345";
    public static readonly string TradingName = "Test Business Ltd";
    public static readonly string AddressLine1 = "Test Adr Line 1";
    public static readonly string Postcode = "TE11 1ST";
    public static readonly string UPRN = "1234567890";
    public static readonly string MCSNumber = "AAA-54545";
    public static readonly string InstallerEmail = "Testinstaller@bus.com";
    public static readonly string UserEmail = "Testuser@bus.com";

    public static readonly Guid ExternalUserAccountId = Guid.NewGuid();

    public static BusinessAddress BusAddress => new()
    {
        AddressLine1 = "Test Adr Line 1",
        Postcode = "TE11 1ST",
        UPRN = "1234567890",
        CreatedBy = "Test User",
        CreatedDate = DateTime.Now,
        AddressLine2 = "Line 2",
        AddressLine3 = "Line 3",
        AddressLine4 = "Line 4",
        AddressType = new() { Id = Guid.NewGuid(), Description = "AddressType" },
        LastUpdatedBy = "Updated",
        LastUpdatedDate = DateTime.Now
    };

    public static CompaniesHouseDetail CompaniesHouseDetail => new()
    {
        CompanyName = BusinessAccountName,
        CompanyNumber = BusinessAccountNumber,
        CompanyRegistrationNumber = BusinessAccountNumber
    };

    protected static BusinessAccount TestBusinessAccount => new()
    {
        Id = BusinessAccountId,
        BusinessName = BusinessAccountName,
        TradingName = TradingName,
        MCSCertificationNumber = MCSNumber,
        AccountSetupRequestDate = new DateTime(2022, 05, 30),
        ActiveDate = new DateTime(2022, 05, 30),
        CreatedDate = new DateTime(2022, 05, 30),
        CreatedBy = InstallerEmail,
        IsUnderInvestigation = false,
        BusinessAccountNumber = $"BUS{BusinessAccountNumber}",
        BusinessAddresses = new List<BusinessAddress>() { BusAddress },
        SubStatus = ActiveSubStatus,
        SubStatusId = ActiveSubStatus.Id
    };

    protected static ExternalUserAccount ExternalUserAccount => new()
    {
        Id = ExternalUserAccountId,
        AuthorisedRepresentative = true,
        SuperUser = true,
        EmailAddress = "TestSuperUser@ofgem.gov.uk",
        TelephoneNumber = "01234567890",
        CreatedDate = new DateTime(2022, 08, 13),
        StandardUser = false
    };

    protected static BusinessAccountSubStatus FailedSubStatus => new()
    {
        Id = Guid.Parse("C42A347D-B5F1-4BF4-924C-72ADCBFD56A1"),
        Description = "Failed",
        Code = BusinessAccountSubStatus.BusinessAccountSubStatusCode.FAIL,
        DisplayName = "Failed"
    };

    protected static BusinessAccountSubStatus WithdrawnSubStatus => new()
    {
        Id = Guid.Parse("549E0D4D-694D-4BBB-B4F7-5AB8525A6E8D"),
        Description = "Withdrawn",
        Code = BusinessAccountSubStatus.BusinessAccountSubStatusCode.WITHDR,
        DisplayName = "Withdrawn"
    };

    protected static BusinessAccountSubStatus RevokedSubStatus => new()
    {
        Id = Guid.Parse("1480C459-3414-44FC-8BDB-003A87F71CCB"),
        Description = "Revoked",
        Code = BusinessAccountSubStatus.BusinessAccountSubStatusCode.REVOK,
        DisplayName = "Revoked"
    };

    protected static BusinessAccountSubStatus ActiveSubStatus => new()
    {
        Id = Guid.Parse("16671635-2BF2-4056-8FDA-227A6AE3B631"),
        Description = "Active",
        Code = BusinessAccountSubStatus.BusinessAccountSubStatusCode.ACTIV,
        DisplayName = "Active"
    };

    protected static readonly List<ExternalUserAccount> ExternalUserAccounts = new() { ExternalUserAccount };

    protected static readonly Invite Invite = new()
    {
        ID = Guid.Empty,
        ExternalUserAccountId = ExternalUserAccount.Id,
        AccountName = ExternalUserAccount.FirstName + " " + ExternalUserAccount.LastName,
        FullName = ExternalUserAccount.FirstName + " " + ExternalUserAccount.LastName,
        EmailAddress = ExternalUserAccount.EmailAddress,
        SentOn = DateTime.Now,
        ExpiresOn = DateTime.Now.AddDays(7)
    };

    protected BusinessAccountsService GenerateSystemUnderTest() => new(
        _mockMapper.Object,
        _mockBusinessAccountsProvider.Object
        );

    protected static void Add_GetBusinessAccountAuthorisedRepresentativeEmailByIdAsync_ToMockedProvider(ref Mock<IBusinessAccountProvider> mock)
    {
        mock
            .Setup(m => m.GetBusinessAccountAuthorisedRepresentativeEmailByIdAsync(BusinessAccountId))
            .ReturnsAsync(InstallerEmail);
    }

    protected static void Add_GetUsersByBusinessAccountIdAsync_ToMockedProvider(ref Mock<IBusinessAccountProvider> mock)
    {
        mock
            .Setup(m => m.GetUsersByBusinessAccountIdAsync(BusinessAccountId))
            .ReturnsAsync(ExternalUserAccounts);
    }

    protected static void Add_AddNewBusinessAccountReferenceNumberAsync_ToMockedProvider(ref Mock<IBusinessAccountProvider> mock)
    {
        mock
            .Setup(m => m.GetNewBusinessAccountNumberAsync())
            .ReturnsAsync(12345);
    }

    protected static void Add_GetFullBusinessAccountByIdAsync_ToMockedProvider(ref Mock<IBusinessAccountProvider> mock)
    {
        mock
            .Setup(m => m.GetFullBusinessAccountById(BusinessAccountId))
            .ReturnsAsync(TestBusinessAccount);
    }

    protected static void Add_UpdateBusinessAccount_ToMockedProvider(ref Mock<IBusinessAccountProvider> mock)
    {
        mock
            .Setup(m => m.UpdateBusinessAccountAsync(It.IsAny<BusinessAccount>()))
            .ReturnsAsync(TestBusinessAccount);
    }

    protected static void Add_GetBusinessAccountsForMcsNumber_ToMockedProvider(ref Mock<IBusinessAccountProvider> mock, BusinessAccount businessAccount)
    {
        var testList = new List<BusinessAccount>
        {
            businessAccount
        };

        mock
             .Setup(m => m.GetBusinessAccountsForMcsNumber(MCSNumber))
             .ReturnsAsync(testList);
    }

    protected static void Add_GetBusinessAccountsSubStatusesListAsync_ToMockedProvider(ref Mock<IBusinessAccountProvider> mock)
    {
        var testList = new List<BusinessAccountSubStatus>
        {
            new BusinessAccountSubStatus { Code = BusinessAccountSubStatus.BusinessAccountSubStatusCode.ACTIV}
        };

        mock.Setup(m => m.GetBusinessAccountsSubStatusesListAsync()).ReturnsAsync(testList);
    }

    protected static void Add_GetBusinessAccountsForMCSNumber_Empty_ToMockedProvider(ref Mock<IBusinessAccountProvider> mock, string mcsNumber)
    {
        var testList = new List<BusinessAccount>();

        mock
            .Setup(m => m.GetBusinessAccountsForMcsNumber(mcsNumber))
            .ReturnsAsync(testList);
    }

    protected static void Add_CreateInvite_ToMockedProvider(ref Mock<IBusinessAccountProvider> mock)
    {
        mock
            .Setup(m => m.CreateInviteAsync(Invite))
            .ReturnsAsync(Invite);
    }

    protected static void Add_GetIndividualInvite_ToMockedProvider(ref Mock<IBusinessAccountProvider> mock)
    {
        mock
            .Setup(m => m.GetInviteAsync(It.IsAny<Guid>()))
            .ReturnsAsync(Invite);
    }

    protected static void Add_GetAllInvitesForUser_ToMockedProvider(ref Mock<IBusinessAccountProvider> mock)
    {
        List<Invite> testList = new() { Invite };

        mock
            .Setup(m => m.GetUserInvitesAsync(It.IsAny<Guid>()))
            .ReturnsAsync(testList);
    }
}
