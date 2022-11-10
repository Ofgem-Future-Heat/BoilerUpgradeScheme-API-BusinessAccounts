using FluentAssertions;
using NUnit.Framework;
using Ofgem.API.BUS.BusinessAccounts.Domain.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Ofgem.API.BUS.BusinessAccounts.Core.UnitTests.BusinessAccountsServiceTests;

/// <summary>
/// Tests for BusinessAccountsService's implementation of 
/// IBusinessAccountsService.GetBusinessAccountById
/// </summary>
public class GetBusinessAccountByIdTests : BusinessAccountsServiceTestsBase
{

    [Test]
    public async Task ValidateGetBusinessAccountById200()
    {
        //Arrange
        var expected = DbContext.BusinessAccounts.First(x => x.MCSMembershipNumber == "MCS128373");
        //Act
        var actual = await _accountsService.GetBusinessAccountById(expected.Id);
        //Assert
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void ValidateGetBusinessAccountByIdNoGuid()
    {
        //Arrange
        var guid = Guid.Empty;

        //Act
        var result = () => _accountsService.GetBusinessAccountById(guid);

        //Assert
        result.Should().ThrowAsync<BadRequestException>();
    }

    [Test]
    public void ValidateGetBusinessAccountNoneFound()
    {
        //arrange
        var guid = new Guid("07c9d38d-7c68-408a-9894-754e07cf9a92");

        //act
        var result = () => _accountsService.GetBusinessAccountById(guid);

        //Assert
        result.Should().ThrowAsync<BadRequestException>();
    }
}
