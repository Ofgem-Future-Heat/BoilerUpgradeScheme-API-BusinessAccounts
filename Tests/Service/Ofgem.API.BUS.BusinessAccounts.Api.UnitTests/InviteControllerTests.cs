using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Ofgem.API.BUS.BusinessAccounts.Core.Interfaces;
using Ofgem.API.BUS.BusinessAccounts.Domain.CommsObjects;
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
    public class InviteControllerTests : BusinessAccountBaseTests
    {
        private InviteController _inviteController;
        private readonly Mock<IInviteService> _inviteServiceMock = new();
        private BusinessAccount _businessAccount;
        private ExternalUserAccount _externalUserAccount;
        private Invite _invite;
        private Invite _additionalInvite;
        private PostInviteRequest _postInviteRequest;

        [SetUp]
        public new void Setup()
        {
            var inviteServiceMock = _inviteServiceMock.Object;
            var accountsServiceMock = _mockAccountsService.Object;
            _inviteController = new InviteController(inviteServiceMock, accountsServiceMock)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };

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

            _externalUserAccount = new()
            {
                Id = ExternalUserAccount.Id,
                FirstName = ExternalUserAccount.FirstName,
                LastName = ExternalUserAccount.LastName,
                TelephoneNumber = ExternalUserAccount.TelephoneNumber,
                EmailAddress = ExternalUserAccount.EmailAddress,
                SuperUser = ExternalUserAccount.SuperUser,
                StandardUser = ExternalUserAccount.StandardUser,
                BusinessAccountID = BusinessAccountId,
                CreatedBy = InstallerEmail,
                CreatedDate = DateTime.Now,
                HomeAddress = ExternalUserAccount.HomeAddress
            };

            _invite = new()
            {
                ID = Invite.ID,
                ExternalUserAccountId = _externalUserAccount.Id,
                AccountName = _externalUserAccount.FirstName + " " + _externalUserAccount.LastName,
                FullName = _externalUserAccount.FirstName + " " + _externalUserAccount.LastName,
                EmailAddress = _externalUserAccount.EmailAddress,
                SentOn = DateTime.Now,
                ExpiresOn = DateTime.Now.AddDays(7)
            };

            _additionalInvite = new()
            {
                ID = new Guid("da3b2ecb-8ea2-47da-be0e-27f5dbdb9a18"),
                ExternalUserAccountId = _externalUserAccount.Id,
                AccountName = _externalUserAccount.FirstName + " " + _externalUserAccount.LastName,
                FullName = _externalUserAccount.FirstName + " " + _externalUserAccount.LastName,
                EmailAddress = _externalUserAccount.EmailAddress,
                SentOn = DateTime.Now,
                ExpiresOn = DateTime.Now.AddDays(7)
            };

            _postInviteRequest = new()
            {
                BusinessAccount = _businessAccount,
                BusinessAccountId = _businessAccount.Id,
                ExternalUserAccount = _externalUserAccount,
                ExternalUserAccountId = _externalUserAccount.Id,
                InviteId = _invite.ID,
                Invite = _invite
            };
        }

        [Test]
        public void InviteController_Ctor_Valid_NoException()
        {
            var result = () => new InviteController(_inviteServiceMock.Object, _mockAccountsService.Object);

            result.Should().NotThrow();
        }

        [Test]
        public void InviteController_Ctor_NoInviteService_Exception()
        {
            var result = () => new InviteController(null!, _mockAccountsService.Object);

            result.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'inviteService')");
        }

        [Test]
        public void InviteController_Ctor_NoAccountsService_NoException()
        {
            var result = () => new InviteController(_inviteServiceMock.Object, null!);

            result.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'accountsService')");
        }

        [Test]
        public async Task CreateInvite_Success()
        {
            var expectedResult = _inviteController.Ok();
            _inviteServiceMock.Setup(m => m.SendInviteEmailAsync(It.IsAny<PostInviteRequest>())).ReturnsAsync("<GovNotifyId>");
            _inviteServiceMock.Setup(m => m.CreateInviteAsync(_invite)).ReturnsAsync(_invite);
            _inviteServiceMock.Setup(m => m.UpdateInviteAsync(_invite)).ReturnsAsync(_invite);
            _mockAccountsService.Setup(x => x.GetBusinessAccountById(_postInviteRequest.BusinessAccountId)).ReturnsAsync(_postInviteRequest.BusinessAccount);
            _mockAccountsService.Setup(x => x.GetExternalUserAccountById(_postInviteRequest.ExternalUserAccountId)).ReturnsAsync(_postInviteRequest.ExternalUserAccount);

            var result = await _inviteController.CreateInviteAsync(_postInviteRequest);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task CreateInvite_UserNotFound_Throws()
        {
            var expected = new BadRequestObjectResult(new { title = AccountsExceptionMessages.NoAccountFound, status = HttpStatusCode.BadRequest });
            _mockAccountsService.Setup(x => x.GetExternalUserAccountById(_postInviteRequest.ExternalUserAccountId)).ThrowsAsync(new BadRequestException(AccountsExceptionMessages.NoAccountFound));

            var result = await _inviteController.CreateInviteAsync(_postInviteRequest);

            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task CreateInvite_BusinessNotFound_Throws()
        {
            var expected = new BadRequestObjectResult(new { title = AccountsExceptionMessages.NoAccountFound, status = HttpStatusCode.BadRequest });
            _mockAccountsService.Setup(x => x.GetBusinessAccountById(_postInviteRequest.BusinessAccountId)).ThrowsAsync(new BadRequestException(AccountsExceptionMessages.NoAccountFound));
            _mockAccountsService.Setup(x => x.GetExternalUserAccountById(_postInviteRequest.ExternalUserAccountId)).ReturnsAsync(_postInviteRequest.ExternalUserAccount);

            var result = await _inviteController.CreateInviteAsync(_postInviteRequest);

            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task CreateInvite_CreateException_Throws()
        {
            var expected = new BadRequestObjectResult(new { title = "TEST", status = HttpStatusCode.BadRequest });
            _inviteServiceMock.Setup(m => m.CreateInviteAsync(_invite)).Throws(new BadRequestException("TEST"));
            _inviteServiceMock.Setup(m => m.UpdateInviteAsync(_invite)).ReturnsAsync(_invite);
            _mockAccountsService.Setup(x => x.GetBusinessAccountById(_postInviteRequest.BusinessAccountId)).ReturnsAsync(_postInviteRequest.BusinessAccount);
            _mockAccountsService.Setup(x => x.GetExternalUserAccountById(_postInviteRequest.ExternalUserAccountId)).ReturnsAsync(_postInviteRequest.ExternalUserAccount);

            var result = await _inviteController.CreateInviteAsync(_postInviteRequest);

            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task CreateInvite_UpdateException_Throws()
        {
            var expected = new BadRequestObjectResult(new { title = "TEST", status = HttpStatusCode.BadRequest });
            _inviteServiceMock.Setup(m => m.CreateInviteAsync(_invite)).ReturnsAsync(_invite);
            _inviteServiceMock.Setup(m => m.UpdateInviteAsync(_invite)).Throws(new BadRequestException("TEST"));
            _mockAccountsService.Setup(x => x.GetBusinessAccountById(_postInviteRequest.BusinessAccountId)).ReturnsAsync(_postInviteRequest.BusinessAccount);
            _mockAccountsService.Setup(x => x.GetExternalUserAccountById(_postInviteRequest.ExternalUserAccountId)).ReturnsAsync(_postInviteRequest.ExternalUserAccount);

            var result = await _inviteController.CreateInviteAsync(_postInviteRequest);

            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task CreateInvite_SendException_Throws()
        {
            var expected = new BadRequestObjectResult(new { title = "TEST", status = HttpStatusCode.BadRequest });
            _inviteServiceMock.Setup(m => m.SendInviteEmailAsync(It.IsAny<PostInviteRequest>())).Throws(new BadRequestException("TEST"));
            _inviteServiceMock.Setup(m => m.CreateInviteAsync(_invite)).ReturnsAsync(_invite);
            _inviteServiceMock.Setup(m => m.UpdateInviteAsync(_invite)).ReturnsAsync(_invite);
            _mockAccountsService.Setup(x => x.GetBusinessAccountById(_postInviteRequest.BusinessAccountId)).ReturnsAsync(_postInviteRequest.BusinessAccount);
            _mockAccountsService.Setup(x => x.GetExternalUserAccountById(_postInviteRequest.ExternalUserAccountId)).ReturnsAsync(_postInviteRequest.ExternalUserAccount);

            var result = await _inviteController.CreateInviteAsync(_postInviteRequest);

            result.Should().BeEquivalentTo(expected);
        }


        [Test]
        public async Task GetAllInvitesAsync_Success()
        {
            var expected = _inviteController.Ok();
            _inviteServiceMock.Setup(m => m.GetAllInvitesAsync()).ReturnsAsync(new[] { _invite, _additionalInvite });

            var result = await _inviteController.GetAllInvitesAsync();

            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetAllInvitesAsync_GetException_Throws()
        {
            var expected = new BadRequestObjectResult(new { title = "TEST", status = HttpStatusCode.BadRequest });
            _inviteServiceMock.Setup(m => m.GetAllInvitesAsync()).Throws(new BadRequestException("TEST"));

            var result = await _inviteController.GetAllInvitesAsync();

            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetInviteAsync_Success()
        {
            var expected = _inviteController.Ok();
            _inviteServiceMock.Setup(m => m.GetInviteAsync(_invite.ID)).ReturnsAsync(_invite);

            var result = await _inviteController.GetInviteAsync(_invite.ID);

            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetInviteAsync_GetException_Throws()
        {
            var expected = new BadRequestObjectResult(new { title = "TEST", status = HttpStatusCode.BadRequest });
            _inviteServiceMock.Setup(m => m.GetInviteAsync(_invite.ID)).Throws(new BadRequestException("TEST"));

            var result = await _inviteController.GetInviteAsync(_invite.ID);

            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetInvitesForUserAsync_Success()
        {
            var expected = _inviteController.Ok();
            _inviteServiceMock.Setup(m => m.GetUserInvitesAsync(_invite.ID)).ReturnsAsync(new List<Invite>() { _invite });

            var result = await _inviteController.GetInvitesForUserAsync(_invite.ID);

            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetInvitesForUserAsync_GetException_Throws()
        {
            var expected = new BadRequestObjectResult(new { title = "TEST", status = HttpStatusCode.BadRequest });
            _inviteServiceMock.Setup(m => m.GetUserInvitesAsync(_invite.ID)).Throws(new BadRequestException("TEST"));

            var result = await _inviteController.GetInvitesForUserAsync(_invite.ID);

            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task UpdateInviteAsync_Success()
        {
            var expected = _inviteController.Ok();
            _inviteServiceMock.Setup(m => m.UpdateInviteAsync(_invite)).ReturnsAsync(_invite);

            var result = await _inviteController.UpdateInviteAsync(_invite.ID, _invite);

            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task UpdateInviteAsync_UpdateException_Throws()
        {
            var expected = new BadRequestObjectResult(new { title = "TEST", status = HttpStatusCode.BadRequest });
            _inviteServiceMock.Setup(m => m.UpdateInviteAsync(_invite)).Throws(new BadRequestException("TEST"));

            var result = await _inviteController.UpdateInviteAsync(_invite.ID, _invite);

            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetInvitesForBusinessAccountAsync_Success()
        {
            var expectedResult = _inviteController.Ok();
            _inviteServiceMock.Setup(m => m.GetBusinessAccountInvitesAsync(_businessAccount.Id)).ReturnsAsync(new[] { _invite, _additionalInvite });

            var result = await _inviteController.GetInvitesForBusinessAccountAsync(_businessAccount.Id);

            result.Should().BeEquivalentTo(expectedResult);
            var okayResult = result as OkObjectResult;
            var resultBod = okayResult?.Value as IEnumerable<Invite>;

            resultBod.Should().BeEquivalentTo(new[] { _invite, _additionalInvite });
        }

        [Test]
        public async Task VerifyTokenAsync_Success()
        {
            TokenVerificationResult expected = new();
            _inviteServiceMock.Setup(m => m.VerifyToken(It.IsAny<string>())).ReturnsAsync(expected);

            var result = await _inviteController.VerifyTokenAsync("x");

            result.Should().BeEquivalentTo(_inviteController.Ok(expected));
        }

        [Test]
        public async Task VerifyTokenAsync_VerifyException_Throws()
        {
            var expected = new BadRequestObjectResult(new { title = "TEST", status = HttpStatusCode.BadRequest });
            _inviteServiceMock.Setup(m => m.VerifyToken(It.IsAny<string>())).Throws(new BadRequestException("TEST"));

            var result = await _inviteController.VerifyTokenAsync("x");

            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task ResendInvite_Success()
        {
            var expectedResult = _inviteController.Ok(true);
            _inviteServiceMock.Setup(m => m.ResendInviteAsync(_invite.ID)).ReturnsAsync(true);

            var result = await _inviteController.ResendInvite(_invite.ID);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task ResendInvite_ResendException_Throws()
        {
            var expected = new BadRequestObjectResult(new { title = "TEST", status = HttpStatusCode.BadRequest });
            _inviteServiceMock.Setup(m => m.ResendInviteAsync(_invite.ID)).Throws(new BadRequestException("TEST"));

            var result = await _inviteController.ResendInvite(_invite.ID);

            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetAllInviteStatus_ReturnsInviteStatus_OnSuccessResponse()
        {
            //Arrange
            var expectedResult = new List<InviteStatus>
                {
                      new InviteStatus{
                        Id= Guid.Parse("1f705c16-74c3-4b46-b3db-48c267beba49"),
                        Code= 0,
                        DisplayName= "Invite Not Sent",
                        Description= "Invite Not Sent"
                      },
                      new InviteStatus{
                        Id= Guid.Parse( "1a853f93-94ce-4ac5-938c-5a943e1fd0f7"),
                        Code= (InviteStatus.InviteStatusCode)5,
                        DisplayName= "Invite Cancelled",
                        Description= "Invite Cancelled"
                      },
                      new InviteStatus{
                        Id= Guid.Parse( "eb989258-b1fe-4733-885d-98f96e95b31c"),
                        Code= (InviteStatus.InviteStatusCode)3,
                        DisplayName=  "Invite Expired",
                         Description=  "Invite Expired"
                      },
                      new InviteStatus{
                       Id= Guid.Parse("9ff91ce7-6a54-47cf-980b-a28687f0ddc0"),
                        Code= (InviteStatus.InviteStatusCode)4,
                      DisplayName= "Signed Up",
                         Description=  "Signed Up"
                      },
                      new InviteStatus{
                        Id= Guid.Parse("d8a3632d-2629-43cc-8bfd-b198db6b97ab"),
                        Code= (InviteStatus.InviteStatusCode)1,
                       DisplayName= "Invited",
                        Description=  "Invited"
                      },
                      new InviteStatus{
                          Id = Guid.Parse("d08080c0-efa3-4e21-be39-daec74a0bb56"),
                          Code = (InviteStatus.InviteStatusCode)2,
                          DisplayName = "Invite Not Delivered",
                         Description=  "Invite Not Delivered"
                      }
                };

            _inviteServiceMock.Setup(x => x.GetAllInviteStatusAsync()).ReturnsAsync(expectedResult);

            //Act
            var result = await _inviteController.GetAllInviteStatusAsync();

            //Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.Should().BeEquivalentTo(new OkObjectResult(expectedResult));
            }
        }

        [Test]
        public async Task GetAllInviteStatus_Exception_ReturnsBadRequest()
        {
            var expected = new BadRequestObjectResult(new { title = "TEST", status = HttpStatusCode.BadRequest });
            _inviteServiceMock.Setup(x => x.GetAllInviteStatusAsync()).ThrowsAsync(new BadRequestException("TEST"));

            var result = await _inviteController.GetAllInviteStatusAsync();

            result.Should().BeEquivalentTo(expected);
        }
    }
}
