using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Ofgem.API.BUS.BusinessAccounts.Domain.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Ofgem.API.BUS.BusinessAccounts.Core.UnitTests.BusinessAccountsServiceTests;

/// <summary>
/// Tests for BusinessAccountsService's implementation of 
/// IBusinessAccountsService.GetFullBusinessAccountById
/// </summary>
public class GetFullBusinessAccountByIdTests : BusinessAccountsServiceTestsBase
{
    [Test]
    public async Task ValidateGetFullBusinessAccountById200()
    {
        //Arrange
        var expected = DbContext.BusinessAccounts.First(x => x.MCSMembershipNumber == "MCS128373");
        //Act
        var actual = await _accountsService.GetFullBusinessAccountById(expected.Id);
        //Assert
        Assert.AreEqual(expected, actual);
    }
    [Test]
    public void ValidateFullGetBusinessAccountByIdNoGuid()
    {
        //Arrange
        var guid = Guid.Empty;

        //Act
        var result = () => _accountsService.GetFullBusinessAccountById(guid);

        //Assert
        result.Should().ThrowAsync<BadRequestException>();
    }
}
