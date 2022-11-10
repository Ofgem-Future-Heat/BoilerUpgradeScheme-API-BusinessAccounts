using FluentAssertions;
using NUnit.Framework;
using Ofgem.API.BUS.BusinessAccounts.Domain.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Ofgem.API.BUS.BusinessAccounts.Core.UnitTests.BusinessAccountsServiceTests;

/// <summary>
/// Tests for BusinessAccountsService's implementation of 
/// IBusinessAccountsService.GetExternalUserAccountById
/// </summary>
public class GetExternalUserAccountByIdTests : BusinessAccountsServiceTestsBase
{
    [Test]
    public async Task ValidateGetExternalUserAccountById200()
    {
        //Arrange
        var expectedId = DbContext.ExternalUserAccounts.Select(x => x.Id).FirstOrDefault();
        //Act
        var actual = await _accountsService.GetExternalUserAccountById(expectedId);
        //Assert
        Assert.AreEqual(expectedId, actual.Id);
    }

    [Test]
    public void ValidateGetExternalUserAccountByIdNoGuid()
    {
        //Arrange
        var guid = Guid.Empty;
        //Act
        var result = () => _accountsService.GetExternalUserAccountById(guid);
        //Assert
        result.Should().ThrowAsync<BadRequestException>();
    }
}
