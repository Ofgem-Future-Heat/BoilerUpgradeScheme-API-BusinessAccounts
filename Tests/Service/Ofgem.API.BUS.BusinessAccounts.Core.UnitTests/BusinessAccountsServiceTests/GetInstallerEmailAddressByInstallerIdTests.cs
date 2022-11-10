using Moq;
using NUnit.Framework;
using Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess.Interfaces;
using System;
using System.Threading.Tasks;

namespace Ofgem.API.BUS.BusinessAccounts.Core.UnitTests.BusinessAccountsServiceTests;

/// <summary>
/// Tests for BusinessAccountsService's implementation of 
/// IBusinessAccountsService.GetInstallerEmailAddressByInstallerID
/// </summary>
public class GetInstallerEmailAddressByInstallerIdTests : BusinessAccountsServiceTestsBase
{
    [Test]
    public async Task GetInstallerEmailAddressByInstallerIDReturns200()
    {
        //Arrange
        string emailIdReturn = "test@test.com";
        var applicationId = Guid.NewGuid();

        _mockBusinessAccountProvider = new Mock<IBusinessAccountProvider>(MockBehavior.Strict);
        _mockBusinessAccountProvider
            .Setup(m => m.GetExternalUserEmailByInstallerId(It.IsAny<Guid>()))
            .ReturnsAsync(emailIdReturn);

        var systemUnderTest = new BusinessAccountsService(
            _mockMapper.Object,
            _mockBusinessAccountProvider.Object);

        //Act
        var actual = await systemUnderTest.GetExternalUserEmailAddressByInstallerID(applicationId);
        //Assert
        Assert.AreEqual(emailIdReturn, actual);
    }
}
