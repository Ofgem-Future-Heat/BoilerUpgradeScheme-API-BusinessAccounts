using FluentAssertions;
using Moq;
using NUnit.Framework;
using Ofgem.API.BUS.BusinessAccounts.Domain.Entities;
using Ofgem.API.BUS.BusinessAccounts.Domain.Exceptions;
using Ofgem.API.BUS.BusinessAccounts.Domain.Request;
using Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Ofgem.API.BUS.BusinessAccounts.Core.UnitTests.BusinessAccountsServiceTests;

public class AddBusinessAccountTests : BusinessAccountsServiceTestsBase
{
    private async Task<string> AddInitialAccount()
    {
        var businessAccount = DbContext.BusinessAccounts.First();

        if (!DbContext.ExternalUserAccounts.Where(x => x.EmailAddress == "duplicate@ofgem.gov.uk").Any())
        {
            var existingUser = new ExternalUserAccount()
            {
                FirstName = "Test",
                LastName = "Testerson",
                EmailAddress = "duplicate@ofgem.gov.uk",
                TelephoneNumber = "01234 567890",
                AuthorisedRepresentative = false,
                SuperUser = true,
                StandardUser = false,
                CreatedBy = "TestUser",
                BusinessAccountID = businessAccount.Id
            };
            await _accountsService.UpdateBusinessAccountAuthorisedRepresentative(existingUser);
        }

        return businessAccount.BusinessName ?? "";
    }

    [Test]
    public async Task AddBusinessAccount_ValidEmail_ShouldPassEmailValidationCheck()
    {
        await AddInitialAccount();

        PostBusinessAccountRequest postBusinessAccountRequest = new()
        {
            AuthorisedRepresentative = new ExternalUserAccount()
            {
                FirstName = "Test",
                LastName = "Testerson",
                EmailAddress = "not_duplicate@ofgem.gov.uk",
                TelephoneNumber = "01234 567890",
                AuthorisedRepresentative = false,
                SuperUser = true,
                StandardUser = false,
                CreatedBy = "TestUser"
            }
        };
        _mockMapper.Setup(x => x.Map<BusinessAccount>(It.IsAny<PostBusinessAccountRequest>())).Returns(new BusinessAccount() { MCSCertificationNumber = null! });
        _accountsService = new BusinessAccountsService(_mockMapper.Object, new BusinessAccountProvider(DbContext));

        var result = async () => await _accountsService.AddBusinessAccount(postBusinessAccountRequest);

        await result.Should().ThrowAsync<BadRequestException>().WithMessage($"The account number provided was not a valid number.");
    }

    [Test]
    public async Task AddBusinessAccount_Duplicate_ShouldThrow()
    {
        var businessAccountName = await AddInitialAccount();

        PostBusinessAccountRequest postBusinessAccountRequest = new()
        {
            AuthorisedRepresentative = new ExternalUserAccount()
            {
                FirstName = "Test",
                LastName = "Testerson",
                EmailAddress = "duplicate@ofgem.gov.uk",
                TelephoneNumber = "01234 567890",
                AuthorisedRepresentative = false,
                SuperUser = true,
                StandardUser = false,
                CreatedBy = "TestUser"
            }
        };

        var result = async() => await _accountsService.AddBusinessAccount(postBusinessAccountRequest);

        await result.Should().ThrowAsync<BadRequestException>().WithMessage($"This email is already in use on {businessAccountName}'s installer account");
    }

}
