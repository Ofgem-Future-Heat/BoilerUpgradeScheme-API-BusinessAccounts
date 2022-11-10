using FluentAssertions;
using NUnit.Framework;
using Ofgem.API.BUS.BusinessAccounts.Domain.Exceptions;
using System.Linq;
using System.Threading.Tasks;

namespace Ofgem.API.BUS.BusinessAccounts.Core.UnitTests.BusinessAccountsServiceTests;

/// <summary>
/// Tests for BusinessAccountsService's implementation of 
/// IBusinessAccountsService.CheckIfAccountExists
/// </summary>
public class CheckIfAccountExistsTests : BusinessAccountsServiceTestsBase
{

    [Test]
    public async Task ValidateNoExistingMcsNumberAppears()
    {
        //Arrange
        var mcsNumber = "-1";
        //Act
        var result = await _accountsService.AccountWithMcsNumberExistsAsync(mcsNumber);

        //Assert
        result.Should().BeFalse();
    }

    [Test]
    public async Task ValidateExistingMcsNumberReturnsTrue()
    {
        //arrange
        var mcsNumber = DbContext.BusinessAccounts.First().MCSCertificationNumber;

        //Act
        var result = await _accountsService.AccountWithMcsNumberExistsAsync(mcsNumber!);

        //Assert
        result.Should().BeTrue();
    }

    [Test]
    public void ValidateErrorsThrownWhenNoMcsNumberProvided()
    {
        //Arrange
        var mcsNumber = "0";

        //Act
        var result = () => _accountsService.AccountWithMcsNumberExistsAsync(mcsNumber);

        //Arrange
        result.Should().ThrowAsync<BadRequestException>();
    }
}
