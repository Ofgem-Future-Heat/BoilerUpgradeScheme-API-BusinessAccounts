using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Notify.Interfaces;
using Notify.Models.Responses;
using Ofgem.API.BUS.BusinessAccounts.Core.Interfaces;
using Ofgem.API.BUS.BusinessAccounts.Domain.CommsObjects;
using Ofgem.API.BUS.BusinessAccounts.Domain.Constants;
using Ofgem.API.BUS.BusinessAccounts.Domain.Entities;
using Ofgem.API.BUS.BusinessAccounts.Domain.Exceptions;
using Ofgem.API.BUS.BusinessAccounts.Domain.Request;
using Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ofgem.API.BUS.BusinessAccounts.Core
{
    public class InviteService : IInviteService
    {
        /// <summary>
        /// Interface for allowing transactions with the database.
        /// </summary>
        private readonly IBusinessAccountProvider _businessAccountProvider;

        private readonly IInviteTokenServiceOptions _inviteTokenServiceOptions;

        private readonly IAccountsService _accountsService;

        private readonly IAsyncNotificationClient _govNotifyClient;

        public InviteService(IMapper mapper, IBusinessAccountProvider businessAccountProvider, IInviteTokenServiceOptions inviteTokenServiceOptions, IAsyncNotificationClient govNotifyClient, IAccountsService accountsService)
        {
            _businessAccountProvider = businessAccountProvider ?? throw new ArgumentNullException(nameof(businessAccountProvider));
            _inviteTokenServiceOptions = inviteTokenServiceOptions ?? throw new ArgumentNullException(nameof(inviteTokenServiceOptions));
            _govNotifyClient = govNotifyClient ?? throw new ArgumentNullException(nameof(govNotifyClient));
            _accountsService = accountsService ?? throw new ArgumentNullException(nameof(accountsService));
        }

        /// <summary>
        /// Method to create a new invite
        /// </summary>
        /// <param name="invite"></param>
        /// <returns></returns>
        public async Task<Invite> CreateInviteAsync(Invite invite)
        {
            return await _businessAccountProvider.CreateInviteAsync(invite);
        }

        /// <summary>
        /// Method to get a specific invite
        /// </summary>
        /// <param name="inviteID"></param>
        /// <returns></returns>
        public async Task<Invite> GetInviteAsync(Guid inviteId)
        {
            return await _businessAccountProvider.GetInviteAsync(inviteId);
        }

        /// <summary>
        /// Method to get all invites for a user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<Invite>> GetUserInvitesAsync(Guid userId)
        {
            return await _businessAccountProvider.GetUserInvitesAsync(userId);
        }

        /// <summary>
        /// Method to update invites
        /// </summary>
        /// <param name="invite"></param>
        /// <returns></returns>
        public async Task<Invite> UpdateInviteAsync(Invite invite)
        {
            return await _businessAccountProvider.UpdateInviteAsync(invite);
        }

        /// <summary>
        /// Method to get all invites for a business account
        /// </summary>
        /// <param name="businessAccountId"></param>
        /// <returns>All invites for a business account</returns>
        public async Task<IEnumerable<Invite>> GetBusinessAccountInvitesAsync(Guid businessAccountId)
            => await _businessAccountProvider.GetBusinessAccountInvitesAsync(businessAccountId);

        /// <summary>
        /// Method to verify token when provided for invite checking
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<TokenVerificationResult> VerifyToken(string token)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (!ValidateToken(token))
            {
                return new TokenVerificationResult()
                {
                    TokenAccepted = false
                };
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            if (tokenHandler.ReadToken(token) is not JwtSecurityToken securityToken)
            {
                return new TokenVerificationResult()
                {
                    TokenAccepted = false
                };
            }

            return new TokenVerificationResult()
            {
                TokenAccepted = true,
                InviteID = Guid.Parse(securityToken.Claims.First(claim => claim.Type == "InviteID").Value),
                InviteTokenExpiryDate = DateTime.Parse(securityToken.Claims.First(claim => claim.Type == "InviteTokenExpiryDate").Value)
            };
        }

        /// <summary>
        /// Method to generate a new invite token
        /// </summary>
        /// <param name="request"></param>
        /// <param name="tokenExpiryDate"></param>
        /// <returns></returns>
        protected string GenerateToken(Invite request, DateTime tokenExpiryDate)
        {
            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_inviteTokenServiceOptions.InviteTokenSecret));
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim("InviteID", request.ID.ToString()),
                new Claim("InviteTokenExpiryDate", tokenExpiryDate.ToString())
                }),
                Expires = tokenExpiryDate,
                SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Method to validate the token generated for invites
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private bool ValidateToken(string token)
        {
            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_inviteTokenServiceOptions.InviteTokenSecret));
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = mySecurityKey,
                    ValidateAudience = false,
                    ValidateIssuer = false
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }

        private Dictionary<string, dynamic> CreateInviteEmailDictionary(PostInviteRequest invite, DateTime tokenExpiryDate)
        {
            return new Dictionary<string, dynamic>()
            {
                {"business name", invite.BusinessAccount.BusinessName ?? "" },
                {"invite url", $"{_inviteTokenServiceOptions.ExternalPortalURL}invite?token={GenerateToken(invite.Invite, tokenExpiryDate)}" },
                {"link expiry", invite.Invite.ExpiresOn.ToString("d MMMM yyyy") }
            };
        }

        public async Task<string> SendInviteEmailAsync(PostInviteRequest request)
        {
            var tokenExpiry = DateTime.UtcNow.AddDays(request.InviteRequestExpiryDays);
            var timespan = new TimeSpan(23, 59, 59);
            tokenExpiry = tokenExpiry.Date + timespan;

            var emailPersonalisation = CreateInviteEmailDictionary(request, tokenExpiry);

            try
            {
                EmailNotificationResponse response = await _govNotifyClient.SendEmailAsync(
                    request.Invite.EmailAddress,
                    _inviteTokenServiceOptions.TokenEmailtemplateId,
                    emailPersonalisation,
                    null,
                    _inviteTokenServiceOptions.InstallerReplyToAddress
                );
                return response.id;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the status of a related invite email on the gov.notify system
        /// </summary>
        /// <param name="govNotifyId"></param>
        /// <returns></returns>
        public async Task<string> CheckInviteEmailStatusAsync(string govNotifyId)
        {
            try
            {
                var response = await _govNotifyClient.GetNotificationByIdAsync(govNotifyId);
                return response.status;
            }
            catch 
            {
                return string.Empty; 
            }
        }

        /// <summary>
        /// Get all Invite statuses
        /// </summary>
        /// <returns>Collection of Invite status objects</returns>
        public async Task<List<InviteStatus>> GetAllInviteStatusAsync()
        {
            return await _businessAccountProvider.GetAllInviteStatusAsync();
        }

        public async Task<bool> ResendInviteAsync(Guid inviteId) 
        {
            Invite foundInvite;
            BusinessAccount foundAccount;
            ExternalUserAccount foundUserAccount;
            PostInviteRequest request;

            foundInvite = await GetInviteAsync(inviteId);
            if (foundInvite == null)
            {
                throw new BadRequestException("No invite found.");
            }

            foundUserAccount = await _accountsService.GetExternalUserAccountById(foundInvite.ExternalUserAccountId);
            if (foundUserAccount == null)
            {
                throw new BadRequestException("No user account found");
            }

            foundAccount = await _accountsService.GetBusinessAccountById(foundUserAccount.BusinessAccountID);
            if (foundAccount == null)
            {
                throw new BadRequestException("No business account found");
            }

            // Update old invite to expire it
            foundInvite.ExpiresOn = DateTime.UtcNow;
            foundInvite.StatusID = StatusMappings.InviteStatus[InviteStatus.InviteStatusCode.CANCELLED].Id;
            _ = await UpdateInviteAsync(foundInvite);

            // Create a new invite
            Invite newInvite = foundInvite;
            newInvite.ID = Guid.NewGuid();
            newInvite.SentOn = DateTime.UtcNow;
            newInvite.ExpiresOn = DateTime.UtcNow.AddDays(7);
            newInvite.EmailAddress = foundUserAccount.EmailAddress;
            newInvite.StatusID = StatusMappings.InviteStatus[InviteStatus.InviteStatusCode.INVITED].Id;

            newInvite = await CreateInviteAsync(newInvite);


            request = new()
            {
                Invite = newInvite,
                InviteId = newInvite.ID,
                BusinessAccount = foundAccount,
                BusinessAccountId = foundAccount.Id,
                ExternalUserAccount = foundUserAccount,
                ExternalUserAccountId = foundAccount.Id,
                InviteRequestExpiryDays = 7,
            };
            var success = await SendInviteEmailAsync(request);
            return !string.IsNullOrEmpty(success);
        }

        public async Task<IEnumerable<Invite>> GetAllInvitesAsync()
        {
            return await _businessAccountProvider.GetAllInvitesAsync();
        }
    }
}
