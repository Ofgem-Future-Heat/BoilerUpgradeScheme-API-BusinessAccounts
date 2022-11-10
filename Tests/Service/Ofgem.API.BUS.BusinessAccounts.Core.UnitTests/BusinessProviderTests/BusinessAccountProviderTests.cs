using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Ofgem.API.BUS.BusinessAccounts.Domain.Constants;
using Ofgem.API.BUS.BusinessAccounts.Domain.Entities;
using Ofgem.API.BUS.BusinessAccounts.Domain.Exceptions;
using Ofgem.API.BUS.BusinessAccounts.Domain.Request;
using Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Ofgem.API.BUS.BusinessAccounts.Domain.Entities.AddressType;
using static Ofgem.API.BUS.BusinessAccounts.Domain.Entities.BankAccountStatus;
using static Ofgem.API.BUS.BusinessAccounts.Domain.Entities.BusinessAccountSubStatus;

namespace Ofgem.API.BUS.Applications.Core.UnitTests.BusinessProviderTests;

public partial class BusinessAccountProviderTests : TestBaseWithSqlite
{
    private BusinessAccountProvider? _businessAccountProvider;

    [SetUp]
    public void SetUp()
    {
        _businessAccountProvider = new BusinessAccountProvider(DbContext);
    }

    [Test]
    public void BusinessAccountProvider_Ctor_Valid_NoException()
    {
        var result = () => new BusinessAccountProvider(DbContext);

        result.Should().NotThrow();
    }

    [Test]
    public void BusinessAccountProvider_Ctor_NoDbContext_Exception()
    {
        var result = () => new BusinessAccountProvider(null!);

        result.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'context')");
    }

    [Test]
    public async Task GetBusinessAccountNameForEmail_NotFound_ReturnsEmptyString()
    {
        string emailAddress = "";
        Guid userId = Guid.NewGuid();
        var result = await _businessAccountProvider!.GetBusinessAccountNameForEmail(emailAddress, userId);

        result.Should().Be("");
    }

    [Test]
    public async Task GetBusinessAccountNameForEmail_FoundSameUser_ReturnsEmptyString()
    {
        var user = DbContext.ExternalUserAccounts.First();

        var result = await _businessAccountProvider!.GetBusinessAccountNameForEmail(user.EmailAddress, user.Id);

        result.Should().Be("");
    }

    [Test]
    public async Task GetBusinessAccountNameForEmail_FoundDiffUser_ReturnsName()
    {
        var user = DbContext.ExternalUserAccounts.First();
        string businessAccountName = DbContext.BusinessAccounts.Where(x => x.Id == user.BusinessAccountID).FirstOrDefault()?.BusinessName ?? "";

        var result = await _businessAccountProvider!.GetBusinessAccountNameForEmail(user.EmailAddress, Guid.Empty);

        result.Should().Be(businessAccountName);
    }

    [Test]
    public async Task GetBusinessAccountsForMcsNumber_ValidNumber()
    {
        string mcsNumber = DbContext.BusinessAccounts.First()?.MCSCertificationNumber ?? "";

        var result = await _businessAccountProvider!.GetBusinessAccountsForMcsNumber(mcsNumber);

        result.Count.Should().BeGreaterThan(0);
    }

    [Test]
    public async Task GetBusinessAccountsForMcsNumber_InvalidNumber()
    {
        string mcsNumber = "xxxxx";

        var result = await _businessAccountProvider!.GetBusinessAccountsForMcsNumber(mcsNumber);

        result.Count.Should().Be(0);
    }

    [Test]
    public async Task AddBusinessAccount_Empty_Fails()
    {
        BusinessAccount businessAccount = new();

        var result = async () => await _businessAccountProvider!.AddBusinessAccount(businessAccount);

        await result.Should().ThrowAsync<DbUpdateException>();
    }

    [Test]
    public async Task AddBusinessAccount_MissingMcsNumber_Throws()
    {
        Guid subStatusId = DbContext.BusinessAccountSubStatuses.First().Id;
        Guid companyTypeId = DbContext.CompanyTypes.First().Id;
        BusinessAccount businessAccount = new()
        {
            Id = Guid.NewGuid(),
            SubStatusId = subStatusId,
            CompanyTypeId = companyTypeId,
            BusinessAccountNumber = "1234",
            BusinessName = "Taylor Wimpey 2",
            TradingName = "Taylor WImpey 2 Limited",
            MCSCertificationBody = "Certification Body",
            MCSMembershipNumber = "MCS928373",
            MCSConsumerCode = "iwejfjiow",
            MCSCompanyType = "4",
            MCSId = "5476h",
            MCSAddressID = Guid.NewGuid(),
            CreatedBy = "Tests"
        };

        var result = async () => await _businessAccountProvider!.AddBusinessAccount(businessAccount);

        await result.Should().ThrowAsync<DbUpdateException>();
    }

    [Test]
    public async Task AddBusinessAccount_Success()
    {
        Guid subStatusId = DbContext.BusinessAccountSubStatuses.First().Id;
        Guid companyTypeId = DbContext.CompanyTypes.First().Id;
        BusinessAccount businessAccount = new()
        {
            Id = Guid.NewGuid(),
            SubStatusId = subStatusId,
            CompanyTypeId = companyTypeId,
            BusinessAccountNumber = "1234",
            BusinessName = "Taylor Wimpey 2",
            TradingName = "Taylor WImpey 2 Limited",
            MCSCertificationNumber = "9253586792",
            MCSCertificationBody = "Certification Body",
            MCSMembershipNumber = "MCS928373",
            MCSConsumerCode = "iwejfjiow",
            MCSCompanyType = "4",
            MCSId = "5476h",
            MCSAddressID = Guid.NewGuid(),
            CreatedBy = "Tests"
        };

        var result = async () => await _businessAccountProvider!.AddBusinessAccount(businessAccount);

        await result.Should().NotThrowAsync();
    }

    [Test]
    public async Task GetSubStatusAsync_Exists_Returns()
    {
        BusinessAccountSubStatusCode subStatusCode = BusinessAccountSubStatus.BusinessAccountSubStatusCode.ACTIV;
        BusinessAccountSubStatus expected = DbContext.BusinessAccountSubStatuses.Where(x => x.Code == subStatusCode).FirstOrDefault()!;

        var result = await _businessAccountProvider!.GetSubStatusAsync(subStatusCode);

        result.Should().Be(expected);
    }

    [Test]
    public async Task GetSubStatusAsync_NotExists_Throws()
    {
        BusinessAccountSubStatusCode subStatusCode = BusinessAccountSubStatus.BusinessAccountSubStatusCode.ACTIV;
        DbContext.BusinessAccountSubStatuses.RemoveRange(DbContext.BusinessAccountSubStatuses.Where(x => x.Code == subStatusCode));
        DbContext.SaveChanges();

        var result = async () => await _businessAccountProvider!.GetSubStatusAsync(subStatusCode);

        await result.Should().ThrowAsync<InvalidOperationException>();
    }

    [Test]
    public async Task GetAddressTypeAsync_Exists_Returns()
    {
        AddressTypeCode addressTypeCode = AddressTypeCode.BIZ;
        AddressType expected = DbContext.AddressTypes.Where(x => x.Code == addressTypeCode).FirstOrDefault()!;

        var result = await _businessAccountProvider!.GetAddressTypeAsync(addressTypeCode);

        result.Should().Be(expected);
    }

    [Test]
    public async Task GetAddressTypeAsync_NotExists_Throws()
    {
        AddressTypeCode addressTypeCode = AddressTypeCode.BIZ;
        DbContext.AddressTypes.RemoveRange(DbContext.AddressTypes.Where(x => x.Code == addressTypeCode));
        DbContext.SaveChanges();

        var result = async () => await _businessAccountProvider!.GetAddressTypeAsync(addressTypeCode);

        await result.Should().ThrowAsync<InvalidOperationException>();
    }

    [Test]
    public async Task GetBankAccountStatusAsync_Exists_Returns()
    {
        BankAccountStatusCode bankAccountStatusCode = BankAccountStatusCode.ACTIVE;
        BankAccountStatus expected = DbContext.BankAccountStatuses.Where(x => x.Code == bankAccountStatusCode).FirstOrDefault()!;

        var result = await _businessAccountProvider!.GetBankAccountStatusAsync(bankAccountStatusCode);

        result.Should().Be(expected);
    }

    [Test]
    public async Task GetBankAccountStatusAsync_NotExists_Throws()
    {
        BankAccountStatusCode bankAccountStatusCode = BankAccountStatusCode.ACTIVE;
        DbContext.BankAccountStatuses.RemoveRange(DbContext.BankAccountStatuses.Where(x => x.Code == bankAccountStatusCode));
        DbContext.SaveChanges();

        var result = async () => await _businessAccountProvider!.GetBankAccountStatusAsync(bankAccountStatusCode);

        await result.Should().ThrowAsync<InvalidOperationException>();
    }

    [Test]
    public async Task AddBusinessAccountUsers_Success()
    {
        List<ExternalUserAccount> externalUsers = new()
        {
            new ExternalUserAccount()
            {
                FirstName = "Test1",
                LastName = "Testerson",
                EmailAddress = "test1@ofgem.gov.uk",
                TelephoneNumber = "01234 567890",
                AuthorisedRepresentative = true,
                SuperUser = true,
                StandardUser = true,
                HomeAddress = "10 Downing Street, London",
                CreatedBy = "Tests"
            },
            new ExternalUserAccount()
            {
                FirstName = "Test2",
                LastName = "Testerson",
                EmailAddress = "test2@ofgem.gov.uk",
                TelephoneNumber = "01234 567890",
                AuthorisedRepresentative = true,
                SuperUser = true,
                StandardUser = true,
                HomeAddress = "10 Downing Street, London",
                CreatedBy = "Tests"
            }
        };
        Guid businessAccountId = DbContext.BusinessAccounts.First().Id;
        int startCount = DbContext.ExternalUserAccounts.Count();

        var result = async () => await _businessAccountProvider!.AddBusinessAccountUsers(externalUsers, businessAccountId);

        await result.Should().NotThrowAsync();
        int endCount = DbContext.ExternalUserAccounts.Count();

        endCount.Should().Be(startCount + 2);
    }

    [Test]
    public async Task GetExternalUserAccountById_Success()
    {
        ExternalUserAccount expected = DbContext.ExternalUserAccounts.First();

        var result = await _businessAccountProvider!.GetExternalUserAccountById(expected.Id);

        result.Should().Be(expected);
    }

    [Test]
    public async Task GetExternalUserAccountById_NotExists_Throws()
    {
        var result = async () => await _businessAccountProvider!.GetExternalUserAccountById(Guid.Empty);

        await result.Should().ThrowAsync<BadRequestException>().WithMessage(AccountsExceptionMessages.NoAccountFound);
    }

    [Test]
    public async Task GetAllBusinessAccounts_Success()
    {
        List<BusinessAccount> expected = DbContext.BusinessAccounts.ToList();

        var result = await _businessAccountProvider!.GetAllBusinessAccounts();

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetAllBusinessAccounts_NotExists_Throws()
    {
        DbContext.BusinessAccounts.RemoveRange(DbContext.BusinessAccounts);
        DbContext.SaveChanges();

        var result = async () => await _businessAccountProvider!.GetAllBusinessAccounts();

        await result.Should().ThrowAsync<BadRequestException>().WithMessage(AccountsExceptionMessages.NoAccountFound);
    }

    [Test]
    public async Task GetBusinessAccountById_Success()
    {
        BusinessAccount expected = DbContext.BusinessAccounts.First();

        var result = await _businessAccountProvider!.GetBusinessAccountById(expected.Id);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetBusinessAccountById_NotExists_Throws()
    {
        var result = async () => await _businessAccountProvider!.GetBusinessAccountById(Guid.Empty);

        await result.Should().ThrowAsync<BadRequestException>().WithMessage(AccountsExceptionMessages.NoAccountFound);
    }

    [Test]
    public async Task GetBusinessAccountAuthorisedRepresentativeEmailByIdAsync_Success()
    {
        ExternalUserAccount expected = DbContext.ExternalUserAccounts.Where(x => x.AuthorisedRepresentative).First();

        var result = await _businessAccountProvider!.GetBusinessAccountAuthorisedRepresentativeEmailByIdAsync(expected.BusinessAccountID);

        result.Should().Be(expected.EmailAddress);
    }

    [Test]
    public async Task GetBusinessAccountAuthorisedRepresentativeEmailByIdAsync_NotExists_Throws()
    {
        var result = async () => await _businessAccountProvider!.GetBusinessAccountAuthorisedRepresentativeEmailByIdAsync(Guid.Empty);

        await result.Should().ThrowAsync<BadRequestException>().WithMessage(AccountsExceptionMessages.NoAuthorisedRepresentativeFound);
    }

    [Test]
    public async Task GetExternalUserEmailByInstallerId_Success()
    {
        ExternalUserAccount expected = DbContext.ExternalUserAccounts.Where(x => x.AuthorisedRepresentative).First();

        var result = await _businessAccountProvider!.GetExternalUserEmailByInstallerId(expected.Id);

        result.Should().Be(expected.EmailAddress);
    }

    [Test]
    public async Task GetExternalUserEmailByInstallerId_NotExists_Throws()
    {
        var result = async () => await _businessAccountProvider!.GetExternalUserEmailByInstallerId(Guid.Empty);

        await result.Should().ThrowAsync<BadRequestException>().WithMessage(AccountsExceptionMessages.NoAuthorisedRepresentativeFound);
    }

    [Test]
    public async Task GetUsersByBusinessAccountIdAsync_Success()
    {
        ExternalUserAccount expected = DbContext.ExternalUserAccounts.First();

        var result = await _businessAccountProvider!.GetUsersByBusinessAccountIdAsync(expected.BusinessAccountID);

        result.Should().BeEquivalentTo(new List<ExternalUserAccount>() { expected });
    }

    [Test]
    public async Task GetUsersByBusinessAccountIdAsync_NotFound_ReturnsEmptyList()
    {
        var result = await _businessAccountProvider!.GetUsersByBusinessAccountIdAsync(Guid.Empty);

        result.Should().BeEquivalentTo(new List<ExternalUserAccount>());
    }

    [Test]
    public async Task GetBusinessAccountsSubStatusesListAsync_Success()
    {
        List<BusinessAccountSubStatus> expected = await DbContext.BusinessAccountSubStatuses.ToListAsync();

        var result = await _businessAccountProvider!.GetBusinessAccountsSubStatusesListAsync();

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task UpdateBusinessAccountStatusOnlyAsync_Success()
    {
        UpdateBusinessAccountStatusRequest request = new() { BusinessAccountId = DbContext.BusinessAccounts.First().Id };

        var result = async () => await _businessAccountProvider!.UpdateBusinessAccountStatusOnlyAsync(request);

        await result.Should().NotThrowAsync();
    }

    [Test]
    public async Task UpdateBusinessAccountStatusOnlyAsync_NotFound_Throws()
    {
        UpdateBusinessAccountStatusRequest request = new() { BusinessAccountId = Guid.Empty };

        var result = async () => await _businessAccountProvider!.UpdateBusinessAccountStatusOnlyAsync(request);

        await result.Should().ThrowAsync<ResourceNotFoundException>().WithMessage($"Business account with ID {request.BusinessAccountId} not found.");
    }

    [Test]
    public async Task UpdateBusinessAccountAsync_NotFound_Throws()
    {
        BusinessAccount businessAccount = new() { Id = Guid.Empty };

        var result = async () => await _businessAccountProvider!.UpdateBusinessAccountAsync(businessAccount);

        await result.Should().ThrowAsync<ResourceNotFoundException>().WithMessage($"Business account not found for {businessAccount.Id}");
    }

    [Test]
    public async Task UpdateBusinessAccountAsync_Success()
    {
        BusinessAccount businessAccount = DbContext.BusinessAccounts.First();

        var result = async () => await _businessAccountProvider!.UpdateBusinessAccountAsync(businessAccount);

        await result.Should().NotThrowAsync();

    }

    [Test]
    public async Task UpdateBusinessAccountUsers_Success_ShouldUpdate()
    {
        ExternalUserAccount user = DbContext.ExternalUserAccounts.First();
        user.FullName = "Updated User";
        List<ExternalUserAccount> expected = new() { user };

        var result = await _businessAccountProvider!.UpdateBusinessAccountUsers(expected);
        ExternalUserAccount updatedUser = DbContext.ExternalUserAccounts.Where(x => x.Id == user.Id).First();

        result.Should().BeEquivalentTo(expected);
        updatedUser.FullName.Should().Be("Updated User");
    }

    [Test]
    public async Task UpdateAuthorisedRepresentative_Success_ShouldUpdate()
    {
        ExternalUserAccount user = DbContext.ExternalUserAccounts.First();
        user.FullName = "Updated User";

        var result = await _businessAccountProvider!.UpdateAuthorisedRepresentative(user);
        ExternalUserAccount updatedUser = DbContext.ExternalUserAccounts.Where(x => x.Id == user.Id).First();

        result.Should().BeEquivalentTo(user);
        updatedUser.FullName.Should().Be("Updated User");
    }

    [Test]
    public async Task UpdateBusinessAccountUser_Success_ShouldUpdate()
    {
        ExternalUserAccount user = DbContext.ExternalUserAccounts.First();
        user.FullName = "Updated User";

        var result = await _businessAccountProvider!.UpdateBusinessAccountUser(user);
        ExternalUserAccount updatedUser = DbContext.ExternalUserAccounts.Where(x => x.Id == user.Id).First();

        result.Should().BeEquivalentTo(user);
        updatedUser.FullName.Should().Be("Updated User");
    }

    [Test]
    public async Task GetFullBusinessAccountById_Success()
    {
        BusinessAccount businessAccount = DbContext.BusinessAccounts.First();

        var result = await _businessAccountProvider!.GetFullBusinessAccountById(businessAccount.Id);

        result.Should().BeEquivalentTo(businessAccount);
    }

    [Test]
    public async Task GetFullBusinessAccountById_NotFound_Throws()
    {
        var result = async () => await _businessAccountProvider!.GetFullBusinessAccountById(Guid.Empty);

        await result.Should().ThrowAsync<BadRequestException>().WithMessage(AccountsExceptionMessages.NoAccountFound);
    }

    [Test]
    public async Task UpdateBusinessAccountOldAsync_NotFound_Throws()
    {
        BusinessAccount businessAccount = new() { Id = Guid.Empty };
        var result = async () => await _businessAccountProvider!.UpdateBusinessAccountOldAsync(businessAccount);

        await result.Should().ThrowAsync<ResourceNotFoundException>().WithMessage(AccountsExceptionMessages.NoAccountFound);
    }

    [Test]
    public async Task UpdateBusinessAccountOldAsync_Success()
    {
        BusinessAccount businessAccount = DbContext.BusinessAccounts.First();

        var result = await _businessAccountProvider!.UpdateBusinessAccountOldAsync(businessAccount);

        result.Should().BeEquivalentTo(businessAccount);
    }


}
