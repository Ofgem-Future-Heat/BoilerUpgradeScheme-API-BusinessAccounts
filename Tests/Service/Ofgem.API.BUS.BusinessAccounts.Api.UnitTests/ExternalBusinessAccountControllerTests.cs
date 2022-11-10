using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Ofgem.API.BUS.BusinessAccounts.Core.Interfaces;
using Ofgem.API.BUS.BusinessAccounts.Domain.Constants;
using Ofgem.API.BUS.BusinessAccounts.Domain.Entities;
using Ofgem.API.BUS.BusinessAccounts.Domain.Exceptions;
using Ofgem.API.BUS.BusinessAccounts.Domain.Request;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Ofgem.API.BUS.BusinessAccounts.Api.UnitTests
{
    public class ExternalBusinessAccountControllerTests : BusinessAccountBaseTests
    {
        private ExternalBusinessAccountsController _businessAccountsController;
        private readonly Mock<IAccountsService> _accountsServiceMock = new();
        private BusinessAccount _businessAccount;

        [SetUp]
        public void Setup()
        {
            var accountsServiceMock = _accountsServiceMock.Object;
            _businessAccountsController = new ExternalBusinessAccountsController(accountsServiceMock);

            _businessAccount = new()
            {
                Id = BusinessAccountId,
                BusinessName = BusinessAccountName,
                TradingName = TradingName,
                MCSCertificationNumber = MCSNumber,
                AccountSetupRequestDate = new DateTime(2022, 05, 30),
                ActiveDate = new DateTime(2022, 05, 30),
                CreatedDate = new DateTime(2022, 05, 30),
                CreatedBy = InstallerEmail,
                IsUnderInvestigation = false,
                BusinessAccountNumber = $"BUS{BusinessAccountNumber}",
                BusinessAddresses = new List<BusinessAddress>() { BusAddress },
                SubStatus = ActiveSubStatus,
                SubStatusId = ActiveSubStatus.Id
            };
        }

        [Test]
        public void ExternalBusinessAccountsController_Ctor_Valid_NoException()
        {
            var result = () => new ExternalBusinessAccountsController(_accountsServiceMock.Object);

            result.Should().NotThrow();
        }

        [Test]
        public void ExternalBusinessAccountsController_Ctor_NoAccountsService_Exception()
        {
            var result = () => new ExternalBusinessAccountsController(null!);

            result.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'accountsService')");
        }

        [Test]
        public async Task ExternalGetBusinessAccountById_Success()
        {
            var expected = _businessAccountsController.Ok(_businessAccount);
            _accountsServiceMock.Setup(x => x.GetFullBusinessAccountById(BusinessAccountId)).ReturnsAsync(_businessAccount);

            var result = await _businessAccountsController.ExternalGetBusinessAccountById(BusinessAccountId);

            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task ExternalGetBusinessAccountById_NullId_ReturnsBadRequest()
        {
            var expected = new BadRequestObjectResult(new { title = AccountsExceptionMessages.NoGuidError, status = HttpStatusCode.BadRequest });

            var result = await _businessAccountsController.ExternalGetBusinessAccountById(null);

            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task ExternalGetBusinessAccountById_EmptyId_ReturnsBadRequest()
        {
            var expected = new BadRequestObjectResult(new { title = AccountsExceptionMessages.NoGuidError, status = HttpStatusCode.BadRequest });

            var result = await _businessAccountsController.ExternalGetBusinessAccountById(Guid.Empty);

            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task ExternalGetUsersByBusinessAccountIdAsync_Success()
        {
            List<ExternalUserAccount> users = new() { new(), new() };
            var expected = _businessAccountsController.Ok(users);
            _accountsServiceMock.Setup(x => x.GetUsersByBusinessAccountIdAsync(BusinessAccountId)).ReturnsAsync(users);

            var result = await _businessAccountsController.ExternalGetUsersByBusinessAccountIdAsync(BusinessAccountId);

            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task ExternalGetUsersByBusinessAccountIdAsync_EmptyId_ReturnsBadRequest()
        {
            var expected = new BadRequestObjectResult(new { title = AccountsExceptionMessages.NoGuidError, status = HttpStatusCode.BadRequest });

            var result = await _businessAccountsController.ExternalGetUsersByBusinessAccountIdAsync(Guid.Empty);

            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetExternalUserAccountById_Success()
        {
            ExternalUserAccount user = new() { Id = Guid.NewGuid() };
            var expected = _businessAccountsController.Ok(user);
            _accountsServiceMock.Setup(x => x.GetExternalUserAccountById(ExternalUserAccountId)).ReturnsAsync(user);

            var result = await _businessAccountsController.GetExternalUserAccountById(ExternalUserAccountId);

            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetExternalUserAccountById_EmptyId_ReturnsBadRequest()
        {
            var expected = new BadRequestObjectResult(new { title = AccountsExceptionMessages.NoGuidError, status = HttpStatusCode.BadRequest });

            var result = await _businessAccountsController.GetExternalUserAccountById(Guid.Empty);

            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task UpdateExternalUsers_Success()
        {
            List<ExternalUserAccount> users = new() { new() { Id= Guid.NewGuid() }, new() { Id = Guid.NewGuid() } };
            var expected = _businessAccountsController.Ok(users);
            _accountsServiceMock.Setup(x => x.UpdateBusinessAccountUsers(users)).ReturnsAsync(users);

            var result = await _businessAccountsController.UpdateExternalUsers(users);

            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task UpdateExternalUsers_Exception_ReturnsBadRequest()
        {
            var expected = new BadRequestObjectResult(new { title = "TEST", status = HttpStatusCode.BadRequest });
            _accountsServiceMock.Setup(x => x.UpdateBusinessAccountUsers(It.IsAny<List<ExternalUserAccount>>())).ThrowsAsync(new BadRequestException("TEST"));

            var result = await _businessAccountsController.UpdateExternalUsers(new());

            result.Should().BeEquivalentTo(expected);
        }
    }
}
