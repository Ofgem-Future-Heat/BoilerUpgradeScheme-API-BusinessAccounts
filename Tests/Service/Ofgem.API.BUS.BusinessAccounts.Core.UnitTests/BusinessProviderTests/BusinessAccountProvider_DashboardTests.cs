using FluentAssertions;
using NUnit.Framework;
using Ofgem.API.BUS.BusinessAccounts.Domain.Entities;
using Ofgem.API.BUS.BusinessAccounts.Domain.Request;
using Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ofgem.API.BUS.Applications.Core.UnitTests.BusinessProviderTests;

public partial class BusinessAccountProviderTests : TestBaseWithSqlite
{
    [Test]
    public async Task GetPagedBusinessAccounts_NoSortBy_Throws()
    {
        var result = async () => await _businessAccountProvider!.GetPagedBusinessAccounts(1, 20, null!);

        await result.Should().ThrowAsync<ArgumentException>().WithMessage("'sortBy' cannot be null or empty. (Parameter 'sortBy')");
    }

    [Test]
    public async Task GetPagedBusinessAccounts_NoData_ReturnsNew()
    {
        PagedResult<BusinessDashboard> expected = new();

        var result = await _businessAccountProvider!.GetPagedBusinessAccounts(1, 20);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetPagedBusinessAccounts_HasData_Returns()
    {
        List<BusinessDashboard> businessDashboards = new()
        {
            new() { BusinessID = Guid.NewGuid().ToString(), BusinessSubStatusCode = "Sub1" },
            new() { BusinessID = Guid.NewGuid().ToString(), BusinessSubStatusCode = "Sub2" }
        };

        PagedResult<BusinessDashboard> expected = new()
        {
            CurrentPage = 1,
            PageCount = 1,
            PageSize = 20,
            RowCount = 2,
            Results = businessDashboards
        };

        DbContext.BusinessDashboards.AddRange(businessDashboards);
        DbContext.SaveChanges();

        var result = await _businessAccountProvider!.GetPagedBusinessAccounts(1, 20);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetPagedBusinessAccounts_FilterData_ReturnsSubset()
    {
        Guid businessId1 = Guid.NewGuid();
        Guid businessId2 = Guid.NewGuid();
        List<BusinessDashboard> businessDashboards = new()
        {
            new() { BusinessID = businessId1.ToString(), BusinessSubStatusCode = "Sub1" },
            new() { BusinessID = businessId2.ToString(), BusinessSubStatusCode = "Sub2" }
        };

        List<BusinessDashboard> filteredBusinessDashboards = new()
        {
            new() { BusinessID = businessId1.ToString(), BusinessSubStatusCode = "Sub1" }
        };

        PagedResult<BusinessDashboard> expected = new()
        {
            CurrentPage = 1,
            PageCount = 1,
            PageSize = 20,
            RowCount = 1,
            Results = filteredBusinessDashboards
        };

        DbContext.BusinessDashboards.AddRange(businessDashboards);
        DbContext.SaveChanges();

        var result = await _businessAccountProvider!.GetPagedBusinessAccounts(1, 20, filterBusinessAcountsStatusBy: new List<string>() { "Sub1" });

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetPagedBusinessAccounts_SearchData_ReturnsSubset()
    {
        Guid businessId1 = Guid.NewGuid();
        Guid businessId2 = Guid.NewGuid();
        List<BusinessDashboard> businessDashboards = new()
        {
            new() { BusinessID = businessId1.ToString(), BusinessSubStatusCode = "Sub1", BusinessName = "Test1" },
            new() { BusinessID = businessId2.ToString(), BusinessSubStatusCode = "Sub2", BusinessName = "Test2" }
        };

        List<BusinessDashboard> filteredBusinessDashboards = new()
        {
            new() { BusinessID = businessId1.ToString(), BusinessSubStatusCode = "Sub1", BusinessName = "Test1" }
        };

        PagedResult<BusinessDashboard> expected = new()
        {
            CurrentPage = 1,
            PageCount = 1,
            PageSize = 20,
            RowCount = 1,
            Results = filteredBusinessDashboards
        };

        DbContext.BusinessDashboards.AddRange(businessDashboards);
        DbContext.SaveChanges();

        var result = await _businessAccountProvider!.GetPagedBusinessAccounts(1, 20, searchBy: "Test1");

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetPagedBusinessAccounts_SearchDataNull_ReturnsAll()
    {
        Guid businessId1 = Guid.NewGuid();
        Guid businessId2 = Guid.NewGuid();
        List<BusinessDashboard> businessDashboards = new()
        {
            new() { BusinessID = businessId1.ToString(), BusinessSubStatusCode = "Sub1", BusinessName = "Test1" },
            new() { BusinessID = businessId2.ToString(), BusinessSubStatusCode = "Sub2", BusinessName = "Test2" }
        };

        PagedResult<BusinessDashboard> expected = new()
        {
            CurrentPage = 1,
            PageCount = 1,
            PageSize = 20,
            RowCount = 2,
            Results = businessDashboards
        };

        DbContext.BusinessDashboards.AddRange(businessDashboards);
        DbContext.SaveChanges();

        var result = await _businessAccountProvider!.GetPagedBusinessAccounts(1, 20, searchBy: null!);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetPagedBusinessAccounts_PagedData_ReturnsPaged()
    {
        Guid businessId1 = Guid.NewGuid();
        Guid businessId2 = Guid.NewGuid();
        List<BusinessDashboard> businessDashboards = new()
        {
            new() { BusinessID = businessId1.ToString(), BusinessSubStatusCode = "Sub1", BusinessName = "Test1" },
            new() { BusinessID = businessId2.ToString(), BusinessSubStatusCode = "Sub2", BusinessName = "Test2" }
        };
        List<BusinessDashboard> pagedDashboards = new()
        {
            new() { BusinessID = businessId1.ToString(), BusinessSubStatusCode = "Sub1", BusinessName = "Test1" }
        };

        PagedResult<BusinessDashboard> expected = new()
        {
            CurrentPage = 1,
            PageCount = 2,
            PageSize = 1,
            RowCount = 2,
            Results = pagedDashboards
        };

        DbContext.BusinessDashboards.AddRange(businessDashboards);
        DbContext.SaveChanges();

        var result = await _businessAccountProvider!.GetPagedBusinessAccounts(1, 1, sortBy: "BusinessName", orderByDescending: false);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetPagedBusinessAccounts_PagedData2_ReturnsPaged()
    {
        Guid businessId1 = Guid.NewGuid();
        Guid businessId2 = Guid.NewGuid();
        List<BusinessDashboard> businessDashboards = new()
        {
            new() { BusinessID = businessId1.ToString(), BusinessSubStatusCode = "Sub1", BusinessName = "Test1" },
            new() { BusinessID = businessId2.ToString(), BusinessSubStatusCode = "Sub2", BusinessName = "Test2" }
        };
        List<BusinessDashboard> pagedDashboards = new()
        {
            new() { BusinessID = businessId2.ToString(), BusinessSubStatusCode = "Sub2", BusinessName = "Test2" }
        };

        PagedResult<BusinessDashboard> expected = new()
        {
            CurrentPage = 2,
            PageCount = 2,
            PageSize = 1,
            RowCount = 2,
            Results = pagedDashboards
        };

        DbContext.BusinessDashboards.AddRange(businessDashboards);
        DbContext.SaveChanges();

        var result = await _businessAccountProvider!.GetPagedBusinessAccounts(2, 1, sortBy: "BusinessName", orderByDescending: false);

        result.Should().BeEquivalentTo(expected);
    }

    private List<BusinessDashboard> SetupTestDashboardDataForSorting()
    {
        List<BusinessDashboard> businessDashboards = new()
        {
            new() { BusinessID = Guid.NewGuid().ToString(), BusinessSubStatusCode = "Sub1", BusinessName = "Test1", BusinessAccountNumber = "B1", ReviewRecommendation = "R1", BeingAmended = "A1", AccountSetupRequestDate = DateTime.UtcNow.AddDays(1).ToString() },
            new() { BusinessID = Guid.NewGuid().ToString(), BusinessSubStatusCode = "Sub3", BusinessName = "Test3", BusinessAccountNumber = "B3", ReviewRecommendation = "R3", BeingAmended = "A3", AccountSetupRequestDate = DateTime.UtcNow.AddDays(3).ToString() },
            new() { BusinessID = Guid.NewGuid().ToString(), BusinessSubStatusCode = "Sub2", BusinessName = "Test2", BusinessAccountNumber = "B2", ReviewRecommendation = "R2", BeingAmended = "A2", AccountSetupRequestDate = DateTime.UtcNow.AddDays(2).ToString() }
        };

        DbContext.BusinessDashboards.AddRange(businessDashboards);
        DbContext.SaveChanges();

        return businessDashboards;
    }

    private static PagedResult<BusinessDashboard> GetExpectedResultsDataForSorting(List<BusinessDashboard> sortedBusinessDashboards)
    {
        PagedResult<BusinessDashboard> expected = new()
        {
            CurrentPage = 1,
            PageCount = 1,
            PageSize = 20,
            RowCount = sortedBusinessDashboards.Count,
            Results = sortedBusinessDashboards
        };
        return expected;
    }

    [Test]
    public async Task GetPagedBusinessAccounts_SortBusinessNameAsc_ReturnsSorted()
    {
        List<BusinessDashboard> businessDashboards = SetupTestDashboardDataForSorting();
        List<BusinessDashboard> sortedBusinessDashboards = businessDashboards.OrderBy(x => x.BusinessName).ToList();
        PagedResult<BusinessDashboard> expected = GetExpectedResultsDataForSorting(sortedBusinessDashboards);

        var result = await _businessAccountProvider!.GetPagedBusinessAccounts(1, 20, sortBy: "BusinessName", orderByDescending: false);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetPagedBusinessAccounts_SortBusinessNameDesc_ReturnsSorted()
    {
        List<BusinessDashboard> businessDashboards = SetupTestDashboardDataForSorting();
        List<BusinessDashboard> sortedBusinessDashboards = businessDashboards.OrderByDescending(x => x.BusinessName).ToList();
        PagedResult<BusinessDashboard> expected = GetExpectedResultsDataForSorting(sortedBusinessDashboards);

        var result = await _businessAccountProvider!.GetPagedBusinessAccounts(1, 20, sortBy: "BusinessName", orderByDescending: true);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetPagedBusinessAccounts_SortBusinessAccountNumberAsc_ReturnsSorted()
    {
        List<BusinessDashboard> businessDashboards = SetupTestDashboardDataForSorting();
        List<BusinessDashboard> sortedBusinessDashboards = businessDashboards.OrderBy(x => x.BusinessAccountNumber).ToList();
        PagedResult<BusinessDashboard> expected = GetExpectedResultsDataForSorting(sortedBusinessDashboards);

        var result = await _businessAccountProvider!.GetPagedBusinessAccounts(1, 20, sortBy: "BusinessAccountNumber", orderByDescending: false);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetPagedBusinessAccounts_SortBusinessAccountNumberDesc_ReturnsSorted()
    {
        List<BusinessDashboard> businessDashboards = SetupTestDashboardDataForSorting();
        List<BusinessDashboard> sortedBusinessDashboards = businessDashboards.OrderByDescending(x => x.BusinessAccountNumber).ToList();
        PagedResult<BusinessDashboard> expected = GetExpectedResultsDataForSorting(sortedBusinessDashboards);

        var result = await _businessAccountProvider!.GetPagedBusinessAccounts(1, 20, sortBy: "BusinessAccountNumber", orderByDescending: true);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetPagedBusinessAccounts_SortSubStatusAsc_ReturnsSorted()
    {
        List<BusinessDashboard> businessDashboards = SetupTestDashboardDataForSorting();
        List<BusinessDashboard> sortedBusinessDashboards = businessDashboards.OrderBy(x => x.BusinessSubStatus).ToList();
        PagedResult<BusinessDashboard> expected = GetExpectedResultsDataForSorting(sortedBusinessDashboards);

        var result = await _businessAccountProvider!.GetPagedBusinessAccounts(1, 20, sortBy: "SubStatus", orderByDescending: false);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetPagedBusinessAccounts_SortSubStatusDesc_ReturnsSorted()
    {
        List<BusinessDashboard> businessDashboards = SetupTestDashboardDataForSorting();
        List<BusinessDashboard> sortedBusinessDashboards = businessDashboards.OrderByDescending(x => x.BusinessSubStatus).ToList();
        PagedResult<BusinessDashboard> expected = GetExpectedResultsDataForSorting(sortedBusinessDashboards);

        var result = await _businessAccountProvider!.GetPagedBusinessAccounts(1, 20, sortBy: "SubStatus", orderByDescending: true);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetPagedBusinessAccounts_SortReviewRecommendationAsc_ReturnsSorted()
    {
        List<BusinessDashboard> businessDashboards = SetupTestDashboardDataForSorting();
        List<BusinessDashboard> sortedBusinessDashboards = businessDashboards.OrderBy(x => x.ReviewRecommendation).ToList();
        PagedResult<BusinessDashboard> expected = GetExpectedResultsDataForSorting(sortedBusinessDashboards);

        var result = await _businessAccountProvider!.GetPagedBusinessAccounts(1, 20, sortBy: "ReviewRecommendation", orderByDescending: false);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetPagedBusinessAccounts_SortReviewRecommendationDesc_ReturnsSorted()
    {
        List<BusinessDashboard> businessDashboards = SetupTestDashboardDataForSorting();
        List<BusinessDashboard> sortedBusinessDashboards = businessDashboards.OrderByDescending(x => x.ReviewRecommendation).ToList();
        PagedResult<BusinessDashboard> expected = GetExpectedResultsDataForSorting(sortedBusinessDashboards);

        var result = await _businessAccountProvider!.GetPagedBusinessAccounts(1, 20, sortBy: "ReviewRecommendation", orderByDescending: true);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetPagedBusinessAccounts_SortAmendmentAsc_ReturnsSorted()
    {
        List<BusinessDashboard> businessDashboards = SetupTestDashboardDataForSorting();
        List<BusinessDashboard> sortedBusinessDashboards = businessDashboards.OrderBy(x => x.BeingAmended).ToList();
        PagedResult<BusinessDashboard> expected = GetExpectedResultsDataForSorting(sortedBusinessDashboards);

        var result = await _businessAccountProvider!.GetPagedBusinessAccounts(1, 20, sortBy: "Amendment", orderByDescending: false);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetPagedBusinessAccounts_SortAmendmentDesc_ReturnsSorted()
    {
        List<BusinessDashboard> businessDashboards = SetupTestDashboardDataForSorting();
        List<BusinessDashboard> sortedBusinessDashboards = businessDashboards.OrderByDescending(x => x.BeingAmended).ToList();
        PagedResult<BusinessDashboard> expected = GetExpectedResultsDataForSorting(sortedBusinessDashboards);

        var result = await _businessAccountProvider!.GetPagedBusinessAccounts(1, 20, sortBy: "Amendment", orderByDescending: true);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetPagedBusinessAccounts_SortAccountSetupRequestDateAsc_ReturnsSorted()
    {
        List<BusinessDashboard> businessDashboards = SetupTestDashboardDataForSorting();
        List<BusinessDashboard> sortedBusinessDashboards = businessDashboards.OrderBy(x => x.AccountSetupRequestDate).ToList();
        PagedResult<BusinessDashboard> expected = GetExpectedResultsDataForSorting(sortedBusinessDashboards);

        var result = await _businessAccountProvider!.GetPagedBusinessAccounts(1, 20, sortBy: "AccountSetupRequestDate", orderByDescending: false);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetPagedBusinessAccounts_SortAccountSetupRequestDateDesc_ReturnsSorted()
    {
        List<BusinessDashboard> businessDashboards = SetupTestDashboardDataForSorting();
        List<BusinessDashboard> sortedBusinessDashboards = businessDashboards.OrderByDescending(x => x.AccountSetupRequestDate).ToList();
        PagedResult<BusinessDashboard> expected = GetExpectedResultsDataForSorting(sortedBusinessDashboards);

        var result = await _businessAccountProvider!.GetPagedBusinessAccounts(1, 20, sortBy: "AccountSetupRequestDate", orderByDescending: true);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetPagedBusinessAccounts_SortUnspecifiedAsc_ReturnsSorted()
    {
        List<BusinessDashboard> businessDashboards = SetupTestDashboardDataForSorting();
        List<BusinessDashboard> sortedBusinessDashboards = businessDashboards.OrderBy(x => x.AccountSetupRequestDate).ToList();
        PagedResult<BusinessDashboard> expected = GetExpectedResultsDataForSorting(sortedBusinessDashboards);

        var result = await _businessAccountProvider!.GetPagedBusinessAccounts(1, 20, orderByDescending: false);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetPagedBusinessAccounts_SortUnspecifiedDesc_ReturnsSorted()
    {
        List<BusinessDashboard> businessDashboards = SetupTestDashboardDataForSorting();
        List<BusinessDashboard> sortedBusinessDashboards = businessDashboards.OrderByDescending(x => x.AccountSetupRequestDate).ToList();
        PagedResult<BusinessDashboard> expected = GetExpectedResultsDataForSorting(sortedBusinessDashboards);

        var result = await _businessAccountProvider!.GetPagedBusinessAccounts(1, 20, orderByDescending: true);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetPagedBusinessAccounts_SortUnknownAsc_ReturnsSorted()
    {
        List<BusinessDashboard> businessDashboards = SetupTestDashboardDataForSorting();
        List<BusinessDashboard> sortedBusinessDashboards = businessDashboards.OrderBy(x => x.AccountSetupRequestDate).ToList();
        PagedResult<BusinessDashboard> expected = GetExpectedResultsDataForSorting(sortedBusinessDashboards);

        var result = await _businessAccountProvider!.GetPagedBusinessAccounts(1, 20, sortBy: "xxxxx", orderByDescending: false);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetPagedBusinessAccounts_SortUnknownDesc_ReturnsSorted()
    {
        List<BusinessDashboard> businessDashboards = SetupTestDashboardDataForSorting();
        List<BusinessDashboard> sortedBusinessDashboards = businessDashboards.OrderByDescending(x => x.AccountSetupRequestDate).ToList();
        PagedResult<BusinessDashboard> expected = GetExpectedResultsDataForSorting(sortedBusinessDashboards);

        var result = await _businessAccountProvider!.GetPagedBusinessAccounts(1, 20, sortBy: "xxxxx", orderByDescending: true);

        result.Should().BeEquivalentTo(expected);
    }
}
