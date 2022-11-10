using AutoMapper;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Ofgem.API.BUS.BusinessAccounts.Domain.Entities;
using Ofgem.API.BUS.BusinessAccounts.Domain.Request;
using Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Ofgem.API.BUS.BusinessAccounts.Domain.Entities.BusinessAccountSubStatus;

namespace Ofgem.API.BUS.BusinessAccounts.Core.UnitTests.BusinessAccountsServiceTests;

/// <summary>
/// Tests for BusinessAccountsService's implementation of 
/// IBusinessAccountsService.GetPagedBusinessAccounts
/// </summary>
//As this now uses a view which cannot really be unit tested we cannot use the base which uses a mock db
public class GetPagedBusinessAccountsTests 
{
    protected readonly IMapper _mapper;
    protected Mock<IBusinessAccountProvider> _mockBusinessAccountProvider = new();
    protected Mock<IMapper> _mockMapper = new();

    protected static PagedResult<BusinessDashboard> PagedBusinessResults => new()
    {
        CurrentPage = 1,
        PageSize = 1,
        PageCount = 1,
        RowCount = 1,
        Results = new List<BusinessDashboard>() { new BusinessDashboard { AccountSetupRequestDate = "2022-06-01" } }
    };

    protected static void Add_GetPagedBusiness_Return_Collection_Provider_Setup(ref Mock<IBusinessAccountProvider> mock, PagedResult<BusinessDashboard> pagedResult)
    {
        mock
            .Setup(m => m.GetPagedBusinessAccounts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>(),
            It.IsAny<List<string>?>(),
            It.IsAny<string?>())).ReturnsAsync(pagedResult);
    }

    protected static void GetPagedBusiness_Return_Collection_Provider_Setup(ref Mock<IBusinessAccountProvider> mock)
    {
        mock
            .Setup(m => m.GetPagedBusinessAccounts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(),
             It.IsAny<bool>(), It.IsAny<List<string>>(), It.IsAny<string?>())).ReturnsAsync(PagedBusinessResults);
    }

    [SetUp]
    public void Setup()
    {
        _mockBusinessAccountProvider = new();
        _mockMapper = new();
    }

    [Test]
    public async Task GetPagedBusinessAccounts_With_Default_Parameters()
    {
        //Arrange
        var mockOfData = new Mock<PagedResult<BusinessDashboard>>();
        Add_GetPagedBusiness_Return_Collection_Provider_Setup(ref _mockBusinessAccountProvider, mockOfData.Object);

        var systemUnderTest = new BusinessAccountsService(_mapper, _mockBusinessAccountProvider.Object);

        //Act
        PagedResult<BusinessDashboard> actual = await systemUnderTest.GetPagedBusinessAccounts().ConfigureAwait(false);

        //Assert
        actual.Results.Count.Should().Be(0);
    }

    [Test]
    public async Task GetPagedBusinessAccounts_With_BusinessAccounts_Statuses()
    {
        //Arrange
        var mockOfData = new Mock<PagedResult<BusinessDashboard>>();
        Add_GetPagedBusiness_Return_Collection_Provider_Setup(ref _mockBusinessAccountProvider, mockOfData.Object);

        var systemUnderTest = new BusinessAccountsService(_mapper, _mockBusinessAccountProvider.Object);

        //Act
        PagedResult<BusinessDashboard> actual = await systemUnderTest.GetPagedBusinessAccounts(1, 20, "AccountSetupRequestDate", true, "QC,ACTIV,INREV", null).ConfigureAwait(false);

        //Assert
        actual.Results.Count.Should().Be(0);
    }

    [Test]
    public async Task GetPagedBusinessAccount_With_Search_String()
    {
        //Arrange
        var mockOfData = new Mock<PagedResult<BusinessDashboard>>();
        Add_GetPagedBusiness_Return_Collection_Provider_Setup(ref _mockBusinessAccountProvider, mockOfData.Object);

        var systemUnderTest = new BusinessAccountsService(_mapper, _mockBusinessAccountProvider.Object);

        //Act
        PagedResult<BusinessDashboard> actual = await systemUnderTest.GetPagedBusinessAccounts(1, 20, "ReviewRecommendation", true, null, "BUS1000001").ConfigureAwait(false);

        //Assert
        actual.Results.Count.Should().Be(0);
    }

    [Test]
    public async Task GetPagedBusinessAccount_With_Invalid_Search_String()
    {
        //Arrange
        var mockOfData = new Mock<PagedResult<BusinessDashboard>>();
        Add_GetPagedBusiness_Return_Collection_Provider_Setup(ref _mockBusinessAccountProvider, mockOfData.Object);

        var systemUnderTest = new BusinessAccountsService(_mapper, _mockBusinessAccountProvider.Object);

        //Act
        PagedResult<BusinessDashboard> actual = await systemUnderTest.GetPagedBusinessAccounts(1, 20, "AccountSetupRequestDate", true, null, @"\SQLQuery").ConfigureAwait(false);

        //Assert - Ignores the string and returns the default
        actual.Results.Count.Should().Be(0);
    }

    [Test]
    public async Task GetPagedBusinessAccounts_With_BusinessAccount_Statuses_And_With_Invalid_Search_String()
    {
        //Arrange
        var mockOfData = new Mock<PagedResult<BusinessDashboard>>();
        Add_GetPagedBusiness_Return_Collection_Provider_Setup(ref _mockBusinessAccountProvider, mockOfData.Object);

        var systemUnderTest = new BusinessAccountsService(_mapper, _mockBusinessAccountProvider.Object);

        //Act
        PagedResult<BusinessDashboard> actual = await systemUnderTest.GetPagedBusinessAccounts(1, 20, "AccountSetupRequestDate", true, "QC,ACTIV,INREV", @"\SQLQuery").ConfigureAwait(false);

        //Assert
        actual.Results.Count.Should().Be(0);
    }

    [Test]
    public async Task GetPagedBusinessAccounts_Return_Collection_Of_BusinessDashBoard_PagedResults()
    {
        //Arrange
        GetPagedBusiness_Return_Collection_Provider_Setup(ref _mockBusinessAccountProvider);

        var systemUnderTest = new BusinessAccountsService(_mapper, _mockBusinessAccountProvider.Object);

        //Act
        PagedResult<BusinessDashboard> actual = await systemUnderTest.GetPagedBusinessAccounts().ConfigureAwait(false);

        //Assert
        actual.Results.Count.Should().Be(1);
    }
}
