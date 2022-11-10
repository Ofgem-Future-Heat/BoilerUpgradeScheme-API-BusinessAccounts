using Moq;
using NUnit.Framework;
using Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess.Interfaces;
using System;
using System.Threading.Tasks;

namespace Ofgem.API.BUS.BusinessAccounts.Core.UnitTests.BusinessAccountsServiceTests;

/// <summary>
/// Tests for BusinessAccountsService's implementation of 
/// IBusinessAccountsService.GetInstallerEmailAddressByID
/// </summary>
public class GetBusinessAccountEmailByIdTests : BusinessAccountsServiceTestsBase
{
    [Test]
    public async Task GetBusinessAccountEmailByIdReturns200()
    {
        //Arrange
        string emailIdReturn = "test@test.com";
        var applicationId = Guid.NewGuid();

        _mockBusinessAccountProvider = new Mock<IBusinessAccountProvider>(MockBehavior.Strict);
        _mockBusinessAccountProvider
            .Setup(m => m.GetBusinessAccountAuthorisedRepresentativeEmailByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(emailIdReturn);

        var systemUnderTest = new BusinessAccountsService(
            _mockMapper.Object,
            _mockBusinessAccountProvider.Object);

        //Act
        var actual = await systemUnderTest.GetBusinessAccountEmailById(applicationId);
        //Assert
        Assert.AreEqual(emailIdReturn, actual);
    }
}
