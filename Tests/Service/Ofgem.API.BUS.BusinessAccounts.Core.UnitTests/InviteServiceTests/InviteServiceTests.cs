using AutoMapper;
using FluentAssertions;
using Moq;
using Notify.Interfaces;
using Notify.Models.Responses;
using NUnit.Framework;
using Ofgem.API.BUS.BusinessAccounts.Core.Interfaces;
using Ofgem.API.BUS.BusinessAccounts.Domain.CommsObjects;
using Ofgem.API.BUS.BusinessAccounts.Domain.Entities;
using Ofgem.API.BUS.BusinessAccounts.Domain.Exceptions;
using Ofgem.API.BUS.BusinessAccounts.Domain.Request;
using Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ofgem.API.BUS.BusinessAccounts.Core.UnitTests.InviteServiceTests
{
    public class TestInviteService : InviteService
    {
        public TestInviteService(IMapper mapper, IBusinessAccountProvider businessAccountProvider, IInviteTokenServiceOptions inviteTokenServiceOptions, IAsyncNotificationClient govNotifyClient, IAccountsService accountsService) 
            : base(mapper, businessAccountProvider, new InviteTokenServiceOptions("EMAIL_TEMPLATE_ID", Guid.NewGuid().ToString(), "https://testurl/", "INVITE_REPLY_ID"), govNotifyClient, accountsService)
        {
        }

        public string GenerateTestToken(Invite request, DateTime tokenExpiryDate) => GenerateToken(request, tokenExpiryDate);
    }

    public class InviteServiceTests
    {
        protected TestInviteService _inviteService;
        private readonly Mock<IBusinessAccountProvider> _mockBusinessAccountProvider = new();
        private readonly Mock<IMapper> _mockMapper = new();
        private readonly Mock<IInviteTokenServiceOptions> _mockInviteTokenServiceOptions = new();
        private readonly Mock<IAsyncNotificationClient> _mockGovNotifyClient = new();
        private readonly Mock<IAccountsService> _mockAccountsService = new();

        [SetUp]
        public void Setup()
        {
            _mockBusinessAccountProvider.Reset();
            _mockGovNotifyClient.Reset();
            _mockAccountsService.Reset();
            _inviteService = new TestInviteService(_mockMapper.Object, _mockBusinessAccountProvider.Object, _mockInviteTokenServiceOptions.Object, _mockGovNotifyClient.Object, _mockAccountsService.Object);
        }

        [Test]
        public void InviteService_Ctor_Valid_NoException()
        {
            var result = () => new InviteService(_mockMapper.Object, _mockBusinessAccountProvider.Object, _mockInviteTokenServiceOptions.Object, _mockGovNotifyClient.Object, _mockAccountsService.Object);

            result.Should().NotThrow();
        }

        [Test]
        public void InviteService_Ctor_NoProvider_Exception()
        {
            var result = () => new InviteService(_mockMapper.Object, null!, _mockInviteTokenServiceOptions.Object, _mockGovNotifyClient.Object, _mockAccountsService.Object);

            result.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'businessAccountProvider')");
        }

        [Test]
        public void InviteService_Ctor_NoInviteTokenService_Exception()
        {
            var result = () => new InviteService(_mockMapper.Object, _mockBusinessAccountProvider.Object, null!, _mockGovNotifyClient.Object, _mockAccountsService.Object);

            result.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'inviteTokenServiceOptions')");
        }

        [Test]
        public void InviteService_Ctor_NoGovNotify_Exception()
        {
            var result = () => new InviteService(_mockMapper.Object, _mockBusinessAccountProvider.Object, _mockInviteTokenServiceOptions.Object, null!, _mockAccountsService.Object);

            result.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'govNotifyClient')");
        }

        [Test]
        public void InviteService_Ctor_NoAccountService_Exception()
        {
            var result = () => new InviteService(_mockMapper.Object, _mockBusinessAccountProvider.Object, _mockInviteTokenServiceOptions.Object, _mockGovNotifyClient.Object, null!);

            result.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'accountsService')");
        }

        [Test]
        public async Task CreateInviteAsync_ReturnsInvite_Successful()
        {
            Invite expected = new()
            {
                ID = Guid.NewGuid(),
                AccountName = "Account",
                EmailAddress = "test@ofgem.gov.uk",
                ExpiresOn = DateTime.UtcNow.AddDays(14),
                ExternalUserAccount = new() { FullName = "TestUser" },
                Status = new() { Description = "Test" }

            };
            _mockBusinessAccountProvider.Setup(x => x.CreateInviteAsync(It.IsAny<Invite>())).ReturnsAsync(expected);

            var result = await _inviteService.CreateInviteAsync(new());

            result.Should().Be(expected);
        }

        [Test]
        public async Task GetInviteAsync_ReturnsInvite_Successful()
        {
            Invite expected = new() { ID = Guid.NewGuid() };
            _mockBusinessAccountProvider.Setup(x => x.GetInviteAsync(It.IsAny<Guid>())).ReturnsAsync(expected);

            var result = await _inviteService.GetInviteAsync(Guid.NewGuid());

            result.Should().Be(expected);
        }

        [Test]
        public async Task GetUserInvitesAsync_ReturnsInvites_Successful()
        {
            List<Invite> expected = new() { new Invite() { ID = Guid.NewGuid() }, new Invite() { ID = Guid.NewGuid() } };
            _mockBusinessAccountProvider.Setup(x => x.GetUserInvitesAsync(It.IsAny<Guid>())).ReturnsAsync(expected);

            var result = await _inviteService.GetUserInvitesAsync(Guid.NewGuid());

            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task UpdateInviteAsync_ReturnsInvite_Successful()
        {
            Invite expected = new() { ID = Guid.NewGuid() };
            _mockBusinessAccountProvider.Setup(x => x.UpdateInviteAsync(It.IsAny<Invite>())).ReturnsAsync(expected);

            var result = await _inviteService.UpdateInviteAsync(new());

            result.Should().Be(expected);
        }

        [Test]
        public async Task GetBusinessAccountInvitesAsync_ReturnsInvite_Successful()
        {
            List<Invite> expected = new() { new Invite() { ID = Guid.NewGuid() }, new Invite() { ID = Guid.NewGuid() } };
            _mockBusinessAccountProvider.Setup(x => x.GetBusinessAccountInvitesAsync(It.IsAny<Guid>())).ReturnsAsync(expected);

            var result = await _inviteService.GetBusinessAccountInvitesAsync(Guid.NewGuid());

            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task VerifyToken_Valid_Success()
        {
            Guid inviteId = Guid.NewGuid();
            DateTime tokenExpiry = DateTime.UtcNow.AddDays(2);
            // Round off to nearest second
            tokenExpiry = tokenExpiry.AddTicks(-(tokenExpiry.Ticks % TimeSpan.TicksPerSecond));
            Invite invite = new()
            {
                ID = inviteId,
                EmailAddress = "test@ofgem.gov.uk",
                ExpiresOn = DateTime.UtcNow.AddDays(2),
            };
            string token = _inviteService.GenerateTestToken(invite, tokenExpiry);
            TokenVerificationResult expected = new() { InviteID = inviteId, TokenAccepted = true, InviteTokenExpiryDate = tokenExpiry };

            var result = await _inviteService.VerifyToken(token);

            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task VerifyToken_NullTokenString_Throws()
        {
            var result = async () => await _inviteService.VerifyToken(null!);

            await result.Should().ThrowAsync<ArgumentNullException>();
        }

        [Test]
        public async Task VerifyToken_InvalidTokenString_Rejects()
        {
            TokenVerificationResult expected = new() { TokenAccepted = false };

            var result = await _inviteService.VerifyToken("xxxxx");

            result.Should().BeEquivalentTo(expected);
        }

        private void SetupEmailTestData(out EmailNotificationResponse response, out PostInviteRequest request, out DateTime tokenExpiry, out Dictionary<string, dynamic> dictionary)
        {
            response = new EmailNotificationResponse() { id = Guid.NewGuid().ToString() };
            request = new()
            {
                Invite = new()
                {
                    EmailAddress = "test@ofgem.gov.uk",
                    ExpiresOn = DateTime.UtcNow.AddDays(2)
                },
                BusinessAccount = new()
                {
                    BusinessName = "TestBusiness"
                },
                InviteRequestExpiryDays = 2,
            };
            tokenExpiry = DateTime.UtcNow.AddDays(request.InviteRequestExpiryDays);
            var timespan = new TimeSpan(23, 59, 59);
            tokenExpiry = tokenExpiry.Date + timespan;

            dictionary = new Dictionary<string, dynamic>()
            {
                {"business name", request.BusinessAccount.BusinessName ?? "" },
                {"invite url", $"https://testurl/invite?token={_inviteService.GenerateTestToken(request.Invite, tokenExpiry)}" },
                {"link expiry", request.Invite.ExpiresOn.ToString("d MMMM yyyy") }
            };
        }

        [Test]
        public async Task SendInviteEmailAsync_Valid_ShouldSendAndReturnId()
        {
            SetupEmailTestData(out EmailNotificationResponse response, out PostInviteRequest request, out DateTime tokenExpiry, out Dictionary<string, dynamic> dictionary);

            _mockGovNotifyClient.Setup(x => x.SendEmailAsync(
                request.Invite.EmailAddress,
                "EMAIL_TEMPLATE_ID",
                dictionary,
                It.IsAny<string>(),
                "INVITE_REPLY_ID"
                )).ReturnsAsync(response);

            var result = await _inviteService.SendInviteEmailAsync(request);

            result.Should().Be(response.id);
        }

        [Test]
        public async Task SendInviteEmailAsync_InvalidEmailTemplate_ShouldFail()
        {
            SetupEmailTestData(out EmailNotificationResponse response, out PostInviteRequest request, out DateTime tokenExpiry, out Dictionary<string, dynamic> dictionary);

            _mockGovNotifyClient.Setup(x => x.SendEmailAsync(
                request.Invite.EmailAddress,
                "INVALID_EMAIL_TEMPLATE_ID",
                dictionary,
                It.IsAny<string>(),
                "INVITE_REPLY_ID"
                )).ReturnsAsync(response);

            var result = await _inviteService.SendInviteEmailAsync(request);

            result.Should().Be(String.Empty);
        }

        [Test]
        public async Task SendInviteEmailAsync_InvalidInviteReplyId_ShouldFail()
        {
            SetupEmailTestData(out EmailNotificationResponse response, out PostInviteRequest request, out DateTime tokenExpiry, out Dictionary<string, dynamic> dictionary);

            _mockGovNotifyClient.Setup(x => x.SendEmailAsync(
                request.Invite.EmailAddress,
                "EMAIL_TEMPLATE_ID",
                dictionary,
                It.IsAny<string>(),
                "INVALID_INVITE_REPLY_ID"
                )).ReturnsAsync(response);

            var result = await _inviteService.SendInviteEmailAsync(request);

            result.Should().Be(String.Empty);
        }

        [Test]
        public async Task CheckInviteEmailStatusAsync_ValidInvite_Successful()
        {
            var govNotifyId = "1ece247c-8d29-4995-bcc7-8b68df6d0e52";
            Notify.Models.Notification notificationResponse = new() { status = "delivered", id = govNotifyId };
           
            _mockGovNotifyClient.Setup(x => x.GetNotificationByIdAsync(It.IsAny<string>())).ReturnsAsync(notificationResponse);
    
            var result = await _inviteService.CheckInviteEmailStatusAsync(govNotifyId);

            result.Should().BeEquivalentTo("delivered");
        }

        [Test]
        public async Task CheckInviteEmailStatusAsync_InvalidInvite_Throws()
        {
            _mockGovNotifyClient.Setup(x => x.GetNotificationByIdAsync(It.IsAny<string>())).ThrowsAsync(new ApplicationException("TEST"));

            var result = await _inviteService.CheckInviteEmailStatusAsync("");

            result.Should().BeEquivalentTo(String.Empty);
        }

        [Test]
        public async Task GetAllInviteStatus_ReturnsInviteStatus_Successful()
        {
            //Arrange
            var expected = new List<InviteStatus>
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
            _mockBusinessAccountProvider.Setup(x => x.GetAllInviteStatusAsync()).ReturnsAsync(expected);

            //Act
            var result = await _inviteService.GetAllInviteStatusAsync();

            //Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task ResendInviteAsync_NotFound_ShouldThrow()
        {
            Invite? invite = null;
            _mockBusinessAccountProvider.Setup(x => x.GetInviteAsync(It.IsAny<Guid>())).ReturnsAsync(invite!);

            var result = async () => await _inviteService.ResendInviteAsync(Guid.NewGuid());

            await result.Should().ThrowAsync<BadRequestException>().WithMessage("No invite found.");
        }

        [Test]
        public async Task ResendInviteAsync_UserNotFound_ShouldThrow()
        {
            Invite invite = new() { ID = Guid.NewGuid() };
            ExternalUserAccount? user = null;
            _mockBusinessAccountProvider.Setup(x => x.GetInviteAsync(It.IsAny<Guid>())).ReturnsAsync(invite);
            _mockAccountsService.Setup(x => x.GetExternalUserAccountById(It.IsAny<Guid>())).ReturnsAsync(user!);

            var result = async () => await _inviteService.ResendInviteAsync(Guid.NewGuid());

            await result.Should().ThrowAsync<BadRequestException>().WithMessage("No user account found");
        }

        [Test]
        public async Task ResendInviteAsync_BusinessNotFound_ShouldThrow()
        {
            Invite invite = new() { ID = Guid.NewGuid() };
            ExternalUserAccount? user = new();
            BusinessAccount? businessAccount = null;
            _mockBusinessAccountProvider.Setup(x => x.GetInviteAsync(It.IsAny<Guid>())).ReturnsAsync(invite);
            _mockAccountsService.Setup(x => x.GetExternalUserAccountById(It.IsAny<Guid>())).ReturnsAsync(user);
            _mockAccountsService.Setup(x => x.GetBusinessAccountById(It.IsAny<Guid>())).ReturnsAsync(businessAccount!);

            var result = async () => await _inviteService.ResendInviteAsync(Guid.NewGuid());

            await result.Should().ThrowAsync<BadRequestException>().WithMessage("No business account found");
        }

        [Test]
        public async Task ResendInviteAsync_EmailFailure()
        {
            Invite invite = new() { ID = Guid.NewGuid() };
            ExternalUserAccount? user = new();
            BusinessAccount? businessAccount = new();
            EmailNotificationResponse emailResponse = new();
            _mockBusinessAccountProvider.Setup(x => x.GetInviteAsync(It.IsAny<Guid>())).ReturnsAsync(invite);
            _mockAccountsService.Setup(x => x.GetExternalUserAccountById(It.IsAny<Guid>())).ReturnsAsync(user);
            _mockAccountsService.Setup(x => x.GetBusinessAccountById(It.IsAny<Guid>())).ReturnsAsync(businessAccount);
            _mockBusinessAccountProvider.Setup(x => x.CreateInviteAsync(It.IsAny<Invite>())).ReturnsAsync(invite);
            _mockGovNotifyClient.Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(emailResponse);

            var result = await _inviteService.ResendInviteAsync(Guid.NewGuid());

            _mockBusinessAccountProvider.Verify(x => x.UpdateInviteAsync(It.IsAny<Invite>()), Times.Once());
            _mockBusinessAccountProvider.Verify(x => x.CreateInviteAsync(It.IsAny<Invite>()), Times.Once());
            _mockGovNotifyClient.Verify(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once());
            result.Should().Be(!string.IsNullOrEmpty(emailResponse.id));
        }

        [Test]
        public async Task ResendInviteAsync_Success()
        {
            Invite invite = new() { ID = Guid.NewGuid() };
            ExternalUserAccount? user = new();
            BusinessAccount? businessAccount = new();
            EmailNotificationResponse emailResponse = new() { id = Guid.NewGuid().ToString() };
            _mockBusinessAccountProvider.Setup(x => x.GetInviteAsync(It.IsAny<Guid>())).ReturnsAsync(invite);
            _mockAccountsService.Setup(x => x.GetExternalUserAccountById(It.IsAny<Guid>())).ReturnsAsync(user);
            _mockAccountsService.Setup(x => x.GetBusinessAccountById(It.IsAny<Guid>())).ReturnsAsync(businessAccount);
            _mockBusinessAccountProvider.Setup(x => x.CreateInviteAsync(It.IsAny<Invite>())).ReturnsAsync(invite);
            _mockGovNotifyClient.Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(emailResponse);

            var result = await _inviteService.ResendInviteAsync(Guid.NewGuid());

            _mockBusinessAccountProvider.Verify(x => x.UpdateInviteAsync(It.IsAny<Invite>()), Times.Once());
            _mockBusinessAccountProvider.Verify(x => x.CreateInviteAsync(It.IsAny<Invite>()), Times.Once());
            _mockGovNotifyClient.Verify(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string,dynamic>>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once());
            result.Should().Be(!string.IsNullOrEmpty(emailResponse.id));
        }
    }
}
