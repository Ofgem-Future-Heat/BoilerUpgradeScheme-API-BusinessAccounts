using FluentAssertions;
using NUnit.Framework;
using Ofgem.API.BUS.BusinessAccounts.Domain.Entities;
using Ofgem.API.BUS.BusinessAccounts.Domain.Exceptions;
using System.Linq;
using System.Threading.Tasks;

namespace Ofgem.API.BUS.BusinessAccounts.Core.UnitTests.BusinessAccountsServiceTests;

public class UpdateBusinessAccountAuthorisedRepresentativeTests : BusinessAccountsServiceTestsBase
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
    public async Task UpdateBusinessAccountAuthorisedRepresentative_Valid()
    {
        await AddInitialAccount();

        var externalUser = new ExternalUserAccount()
        {
            FirstName = "Test",
            LastName = "Testerson",
            EmailAddress = "not_duplicate@ofgem.gov.uk",
            TelephoneNumber = "01234 567890",
            AuthorisedRepresentative = false,
            SuperUser = true,
            StandardUser = false,
            CreatedBy = "TestUser"
        };

        var result = await _accountsService.UpdateBusinessAccountAuthorisedRepresentative(externalUser);

        result.Should().NotBeNull();
    }

    [Test]
    public async Task UpdateBusinessAccountAuthorisedRepresentative_Duplicate_ShouldThrow()
    {
        var businessAccountName = await AddInitialAccount();

        var externalUser = new ExternalUserAccount()
        {
            FirstName = "Test",
            LastName = "Testerson",
            EmailAddress = "duplicate@ofgem.gov.uk",
            TelephoneNumber = "01234 567890",
            AuthorisedRepresentative = false,
            SuperUser = true,
            StandardUser = false,
            CreatedBy = "TestUser"
        };

        var result = async() => await _accountsService.UpdateBusinessAccountAuthorisedRepresentative(externalUser);

        await result.Should().ThrowAsync<BadRequestException>().WithMessage($"This email is already in use on {businessAccountName}'s installer account");
    }

}
