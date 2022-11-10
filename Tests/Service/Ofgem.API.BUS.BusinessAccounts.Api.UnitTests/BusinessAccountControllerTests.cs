using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Ofgem.API.BUS.BusinessAccounts.Core.Interfaces;
using Ofgem.API.BUS.BusinessAccounts.Domain.Entities;
using Ofgem.API.BUS.BusinessAccounts.Domain.Exceptions;
using Ofgem.API.BUS.BusinessAccounts.Domain.Request;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Ofgem.API.BUS.BusinessAccounts.Api.UnitTests;

public class BusinessAccountControllerTests : BusinessAccountBaseTests
{
    private BusinessAccountsController _businessAccountsController;
    private readonly Mock<IAccountsService> _accountsServiceMock = new();
    private PostBusinessAccountRequest _postBusinessAccountRequest;
    private PostUserAccountRequest _postUserAccountRequest;

    [SetUp]
    public new void Setup()
    {
        var accountsServiceMock = _accountsServiceMock.Object;
        _businessAccountsController = new BusinessAccountsController(accountsServiceMock)
        {
            ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
        };

        _postBusinessAccountRequest = new()
        {
            BusinessName = "Test Business",
            TradingName = "Test Trading",
            MCSCertificationNumber = "Napit-9999",
            CompanyNumber = "12345678",
            BusinessAddress = new PostBusinessAccountRequestBusinessAddress()
            {
                AddressLine1 = "10 Downing Street",
                County = "London",
                Postcode = "EC1N 2PB"
            },
            DateAccountReceived = DateTime.UtcNow,
            AuthorisedRepresentative = new(),
            IsUnderInvestigation = false,
            TradingAddress = new PostBusinessAccountRequestBusinessAddress() { },
            BankAccount = new PostBusinessAccountRequestBankAccount() 
            { 
                AccountName = "Account",
                AccountNumber = "12345678",
                SortCode = "123456"
            }
        };

        _postUserAccountRequest = new()
        {
            BusinessAccountID = Guid.NewGuid(),
            ExternalUserAccounts = new List<PostUserAccountRequestExternalUserAccount>() {
                new PostUserAccountRequestExternalUserAccount()
                {
                    FirstName = "Test",
                    LastName = "Testerson",
                    EmailAddress = "test.testerson@ofgem.gov.uk",
                    TelephoneNumber = "01234 567890",
                    AuthorisedRepresentative = true,
                    SuperUser = true,
                    StandardUser = false
                }
            }
        };
    }

    [Test]
    public void BusinessAccountsController_Ctor_Valid_NoException()
    {
        var result = () => new BusinessAccountsController(_accountsServiceMock.Object);

        result.Should().NotThrow();
    }

    [Test]
    public void BusinessAccountsController_Ctor_NoAccountsService_Exception()
    {
        var result = () => new BusinessAccountsController(null!);

        result.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'accountsService')");
    }

    [Test]
    public async Task AddBusinessAccount_Success()
    {
        var businessAccountId = Guid.NewGuid();
        _accountsServiceMock.Setup(x => x.AddBusinessAccount(_postBusinessAccountRequest)).ReturnsAsync(businessAccountId);
        _accountsServiceMock.Setup(x => x.UpdateBusinessAccountAuthorisedRepresentative(_postBusinessAccountRequest.AuthorisedRepresentative, false));
        var expected = _businessAccountsController.Ok(businessAccountId);

        var result = await _businessAccountsController.AddBusinessAccount(_postBusinessAccountRequest);

        _accountsServiceMock.Verify(x => x.UpdateBusinessAccountAuthorisedRepresentative(_postBusinessAccountRequest.AuthorisedRepresentative, false), Times.Once);
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task AddBusinessAccount_GuidExists_ReturnsBadRequest()
    {
        var expected = new BadRequestObjectResult(new { title = "TEST", status = HttpStatusCode.BadRequest });
        _accountsServiceMock.Setup(x => x.AddBusinessAccount(It.IsAny<PostBusinessAccountRequest>())).Throws(new BadRequestException("TEST"));

        var result = await _businessAccountsController.AddBusinessAccount(new());

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task SetupUserAccounts_Success()
    {
        _accountsServiceMock.Setup(x => x.AddBusinessAccountUsers(_postUserAccountRequest, _postUserAccountRequest.BusinessAccountID));
        var expected = _businessAccountsController.Ok();

        var result = await _businessAccountsController.SetupUserAccounts(_postUserAccountRequest.BusinessAccountID, _postUserAccountRequest);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task SetupUserAccounts_Exception_ReturnsBadRequest()
    {
        var expected = new BadRequestObjectResult(new { title = "TEST", status = HttpStatusCode.BadRequest });
        _accountsServiceMock.Setup(x => x.AddBusinessAccountUsers(It.IsAny<PostUserAccountRequest>(), It.IsAny<Guid>())).Throws(new BadRequestException("TEST"));

        var result = await _businessAccountsController.SetupUserAccounts(Guid.NewGuid(), new());

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetUsersByBusinessAccountIdAsync_Success()
    {
        Guid businessAccountId = Guid.NewGuid();
        var accounts = new List<ExternalUserAccount>() { new(), new() };
        _accountsServiceMock.Setup(x => x.GetUsersByBusinessAccountIdAsync(businessAccountId)).ReturnsAsync(accounts);
        var expected = _businessAccountsController.Ok(accounts);

        var result = await _businessAccountsController.GetUsersByBusinessAccountIdAsync(businessAccountId);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetUsersByBusinessAccountIdAsync_Exception_ReturnsNoContent()
    {
        var expected = _businessAccountsController.NoContent();
        _accountsServiceMock.Setup(x => x.GetUsersByBusinessAccountIdAsync(It.IsAny<Guid>())).Throws(new ResourceNotFoundException("TEST"));

        var result = await _businessAccountsController.GetUsersByBusinessAccountIdAsync(Guid.NewGuid());

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetPagedBusinessAccounts_Success()
    {
        var pagedesult = new PagedResult<BusinessDashboard>();
        var expected = _businessAccountsController.Ok(pagedesult);
        _accountsServiceMock.Setup(x => x.GetPagedBusinessAccounts(1, 20, It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(pagedesult);

        var result = await _businessAccountsController.GetPagedBusinessAccounts(1, 20).ConfigureAwait(false);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetPagedBusinessAccounts_Exception_ReturnsBadRequest()
    {
        var expected = new BadRequestObjectResult(new { title = "TEST", status = HttpStatusCode.BadRequest });
        _accountsServiceMock.Setup(x => x.GetPagedBusinessAccounts(1, 20, It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>())).Throws(new BadRequestException("TEST"));

        var result = await _businessAccountsController.GetPagedBusinessAccounts(1, 20);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetAllBusinessAccounts_Success()
    {
        var businessAccounts = new List<BusinessAccount>() { new(), new() };
        var expected = _businessAccountsController.Ok(businessAccounts);
        _accountsServiceMock.Setup(x => x.GetAllBusinessAccounts()).ReturnsAsync(businessAccounts);

        var result = await _businessAccountsController.GetAllBusinessAccounts();

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetAllBusinessAccounts_Exception_ReturnsBadRequest()
    {
        var expected = new BadRequestObjectResult(new { title = "TEST", status = HttpStatusCode.BadRequest });
        _accountsServiceMock.Setup(x => x.GetAllBusinessAccounts()).Throws(new BadRequestException("TEST"));

        var result = await _businessAccountsController.GetAllBusinessAccounts();

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetBusinessAccountById_Success()
    {
        BusinessAccount businessAccount = new() { Id = Guid.NewGuid() };
        var expected = _businessAccountsController.Ok(businessAccount);
        _accountsServiceMock.Setup(x => x.GetBusinessAccountById(businessAccount.Id)).ReturnsAsync(businessAccount);

        var result = await _businessAccountsController.GetBusinessAccountById(businessAccount.Id);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetBusinessAccountById_Exception_ReturnsBadRequest()
    {
        var expected = new BadRequestObjectResult(new { title = "TEST", status = HttpStatusCode.BadRequest });
        _accountsServiceMock.Setup(x => x.GetBusinessAccountById(It.IsAny<Guid>())).Throws(new BadRequestException("TEST"));

        var result = await _businessAccountsController.GetBusinessAccountById(Guid.NewGuid());

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task UpdateBusinessAccountByIdAsync_Success()
    {
        BusinessAccount businessAccount = new() { Id = Guid.NewGuid(), MCSCertificationNumber = "MCS" };
        var expected = _businessAccountsController.Ok(businessAccount);
        _accountsServiceMock.Setup(x => x.AccountWithMcsNumberExistsAsync(businessAccount.MCSCertificationNumber, businessAccount.Id)).ReturnsAsync(false);
        _accountsServiceMock.Setup(x => x.UpdateBusinessAccountAsync(businessAccount)).ReturnsAsync(businessAccount);

        var result = await _businessAccountsController.UpdateBusinessAccountByIdAsync(businessAccount.Id, businessAccount);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task UpdateBusinessAccountByIdAsync_NullBusiness_Throws()
    {
        var result = async () => await _businessAccountsController.UpdateBusinessAccountByIdAsync(Guid.NewGuid(), null!);

        await result.Should().ThrowAsync<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'businessAccount')");
    }

    [Test]
    public async Task UpdateBusinessAccountByIdAsync_InvalidId_Throws()
    {
        BusinessAccount businessAccount = new() { Id = Guid.NewGuid() };

        var result = async () => await _businessAccountsController.UpdateBusinessAccountByIdAsync(Guid.NewGuid(), businessAccount);

        await result.Should().ThrowAsync<BadArgumentException>().WithMessage("Business account IDs do not match");
    }

    [Test]
    public async Task UpdateBusinessAccountByIdAsync_McsExists_ReturnsBadRequest()
    {
        var expected = new BadRequestObjectResult(new { title = "An active account already exists with this MCS number", status = HttpStatusCode.BadRequest });
        BusinessAccount businessAccount = new() { Id = Guid.NewGuid(), MCSCertificationNumber = "MCS" };
        _accountsServiceMock.Setup(x => x.AccountWithMcsNumberExistsAsync(businessAccount.MCSCertificationNumber, businessAccount.Id)).ReturnsAsync(true);

        var result = await _businessAccountsController.UpdateBusinessAccountByIdAsync(businessAccount.Id, businessAccount);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetFullBusinessAccountById_Success()
    {
        BusinessAccount businessAccount = new() { Id = Guid.NewGuid() };
        var expected = _businessAccountsController.Ok(businessAccount);
        _accountsServiceMock.Setup(x => x.GetFullBusinessAccountById(businessAccount.Id)).ReturnsAsync(businessAccount);

        var result = await _businessAccountsController.GetFullBusinessAccountById(businessAccount.Id);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetFullBusinessAccountById_Exception_ReturnsBadRequest()
    {
        var expected = new BadRequestObjectResult(new { title = "TEST", status = HttpStatusCode.BadRequest });
        _accountsServiceMock.Setup(x => x.GetFullBusinessAccountById(It.IsAny<Guid>())).Throws(new BadRequestException("TEST"));

        var result = await _businessAccountsController.GetFullBusinessAccountById(Guid.NewGuid());

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetExternalUserAccountById_Success()
    {
        ExternalUserAccount externalUserAccount = new() { Id = Guid.NewGuid() };
        var expected = _businessAccountsController.Ok(externalUserAccount);
        _accountsServiceMock.Setup(x => x.GetExternalUserAccountById(externalUserAccount.Id)).ReturnsAsync(externalUserAccount);

        var result = await _businessAccountsController.GetExternalUserAccountById(externalUserAccount.Id);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetExternalUserAccountById_Exception_ReturnsBadRequest()
    {
        var expected = new BadRequestObjectResult(new { title = "TEST", status = HttpStatusCode.BadRequest });
        _accountsServiceMock.Setup(x => x.GetExternalUserAccountById(It.IsAny<Guid>())).Throws(new BadRequestException("TEST"));

        var result = await _businessAccountsController.GetExternalUserAccountById(Guid.NewGuid());

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task UpdateExternalUsers_Success()
    {
        List<ExternalUserAccount> externalUsers = new()
        {
            new ExternalUserAccount()
            {
                FirstName = "Test",
                LastName = "Testerson",
                EmailAddress = "test.testerson2@ofgem.gov.uk",
                TelephoneNumber = "01234 567890",
                AuthorisedRepresentative = true,
                SuperUser = true,
                StandardUser = false
            }
        };
        var expected = _businessAccountsController.Ok(externalUsers);
        _accountsServiceMock.Setup(x => x.UpdateBusinessAccountUsers(externalUsers)).ReturnsAsync(externalUsers);

        var result = await _businessAccountsController.UpdateExternalUsers(externalUsers);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task UpdateExternalUsers_Exception_ReturnsBadRequest()
    {
        var expected = new BadRequestObjectResult(new { title = "TEST", status = HttpStatusCode.BadRequest });
        _accountsServiceMock.Setup(x => x.UpdateBusinessAccountUsers(It.IsAny<List<ExternalUserAccount>>())).Throws(new BadRequestException("TEST"));

        var result = await _businessAccountsController.UpdateExternalUsers(new());

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task UpdateAuthorisedReprasentative_Success()
    {
        ExternalUserAccount externalUser = new()
        {
            FirstName = "Test",
            LastName = "Testerson",
            EmailAddress = "test.testerson2@ofgem.gov.uk",
            TelephoneNumber = "01234 567890",
            AuthorisedRepresentative = true,
            SuperUser = true,
            StandardUser = false
        };
        var expected = _businessAccountsController.Ok(externalUser);
        _accountsServiceMock.Setup(x => x.UpdateBusinessAccountAuthorisedRepresentative(externalUser, true)).ReturnsAsync(externalUser);

        var result = await _businessAccountsController.UpdateAuthorisedReprasentative(externalUser);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task UpdateAuthorisedReprasentative_Exception_ReturnsBadRequest()
    {
        var expected = new BadRequestObjectResult(new { title = "TEST", status = HttpStatusCode.BadRequest });
        _accountsServiceMock.Setup(x => x.UpdateBusinessAccountAuthorisedRepresentative(It.IsAny<ExternalUserAccount>(), It.IsAny<bool>())).Throws(new BadRequestException("TEST"));

        var result = await _businessAccountsController.UpdateAuthorisedReprasentative(new());

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task UpdateAdditionalUser_Success()
    {
        ExternalUserAccount externalUser = new()
        {
            FirstName = "Test",
            LastName = "Testerson",
            EmailAddress = "test.testerson2@ofgem.gov.uk",
            TelephoneNumber = "01234 567890",
            AuthorisedRepresentative = false,
            SuperUser = true,
            StandardUser = false
        };
        var expected = _businessAccountsController.Ok(externalUser);
        _accountsServiceMock.Setup(x => x.UpdateBusinessAccountAdditionalUser(externalUser)).ReturnsAsync(externalUser);

        var result = await _businessAccountsController.UpdateAdditionalUser(externalUser);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task UpdateAdditionalUser_Exception_ReturnsBadRequest()
    {
        var expected = new BadRequestObjectResult(new { title = "TEST", status = HttpStatusCode.BadRequest });
        _accountsServiceMock.Setup(x => x.UpdateBusinessAccountAdditionalUser(It.IsAny<ExternalUserAccount>())).Throws(new BadRequestException("TEST"));

        var result = await _businessAccountsController.UpdateAdditionalUser(new());

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetBusinessAccountEmailById_Success()
    {
        string email = "test@ofgem.gov.uk";
        Guid businessAccountId = Guid.NewGuid();
        var expectedResult = _businessAccountsController.Ok(email);
        _accountsServiceMock.Setup(x => x.GetBusinessAccountEmailById(businessAccountId)).ReturnsAsync(email);

        var result = await _businessAccountsController.GetBusinessAccountEmailById(businessAccountId);

        result.Should().BeEquivalentTo(expectedResult);
    }

    [Test]
    public async Task GetBusinessAccountEmailById_Exception_ReturnsBadRequest()
    {
        var expected = new BadRequestObjectResult(new { title = "TEST", status = HttpStatusCode.BadRequest });
        _accountsServiceMock.Setup(x => x.GetBusinessAccountEmailById(It.IsAny<Guid>())).Throws(new BadRequestException("TEST"));

        var result = await _businessAccountsController.GetBusinessAccountEmailById(Guid.NewGuid());

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetExternalUserEmailAddressByInstallerID_Success()
    {
        string email = "test@ofgem.gov.uk";
        Guid installerId = Guid.NewGuid();
        var expectedResult = _businessAccountsController.Ok(email);
        _accountsServiceMock.Setup(x => x.GetExternalUserEmailAddressByInstallerID(installerId)).ReturnsAsync(email);

        var result = await _businessAccountsController.GetInstallerEmailAddressByInstallerID(installerId);

        result.Should().BeEquivalentTo(expectedResult);
    }

    [Test]
    public async Task GetExternalUserEmailAddressByInstallerID_Exception_ReturnsBadRequest()
    {
        var expected = new BadRequestObjectResult(new { title = "TEST", status = HttpStatusCode.BadRequest });
        _accountsServiceMock.Setup(x => x.GetExternalUserEmailAddressByInstallerID(It.IsAny<Guid>())).Throws(new BadRequestException("TEST"));

        var result = await _businessAccountsController.GetInstallerEmailAddressByInstallerID(Guid.NewGuid());

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetBusinessSubStatusesAsync_Success()
    {
        List<BusinessAccountSubStatus> expected = new() { new(), new() };
        _accountsServiceMock.Setup(x => x.GetBusinessAccountsSubStatusesListAsync()).ReturnsAsync(expected);

        var result = await _businessAccountsController.GetBusinessSubStatusesAsync();

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetBusinessSubStatusesAsync_Exception_ReturnsBadRequest()
    {
        _accountsServiceMock.Setup(x => x.GetBusinessAccountsSubStatusesListAsync()).ReturnsAsync(new List<BusinessAccountSubStatus>());

        var result = async () => await _businessAccountsController.GetBusinessSubStatusesAsync();

        await result.Should().ThrowAsync<BadRequestException>().WithMessage("Could not find list of business acocount statuses");
    }

    [Test]
    public async Task UpdateBusinessAccountStatusOnlyAsync_Success()
    {
        Guid businessAccountId = Guid.NewGuid();
        var request = new UpdateBusinessAccountStatusRequest() { BusinessAccountId = businessAccountId, RequestedByUser = "User", StatusId = Guid.NewGuid() };
        _accountsServiceMock.Setup(x => x.UpdateBusinessAccountStatusOnlyAsync(request));

        var result = await _businessAccountsController.UpdateBusinessAccountStatusOnlyAsync(businessAccountId, request);

        result.Should().BeEquivalentTo(_businessAccountsController.Ok());
    }
}
