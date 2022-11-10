using AutoMapper;
using Moq;
using NUnit.Framework;
using Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess;
using Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess.Interfaces;

namespace Ofgem.API.BUS.BusinessAccounts.Core.UnitTests.BusinessAccountsServiceTests;

/// <summary>
/// Base class with Mocks, Helpers, etc. for BusinessAccountsService test classes
/// </summary>
public class BusinessAccountsServiceTestsBase : TestBaseWithSqlite
{
    protected BusinessAccountsService _accountsService;
    protected readonly IMapper _mapper;
    protected Mock<IBusinessAccountProvider> _mockBusinessAccountProvider = new();
    protected Mock<IMapper> _mockMapper = new();

    [SetUp]
    public void Setup()
    {
        _accountsService = new BusinessAccountsService(_mapper, new BusinessAccountProvider(DbContext));
        _mockBusinessAccountProvider = new();
        _mockMapper = new();
    }
}
