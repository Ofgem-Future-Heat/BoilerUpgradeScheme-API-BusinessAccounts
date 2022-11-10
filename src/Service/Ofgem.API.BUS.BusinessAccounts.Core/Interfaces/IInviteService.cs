using Ofgem.API.BUS.BusinessAccounts.Domain.CommsObjects;
using Ofgem.API.BUS.BusinessAccounts.Domain.Entities;
using Ofgem.API.BUS.BusinessAccounts.Domain.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ofgem.API.BUS.BusinessAccounts.Core.Interfaces
{
    public interface IInviteService
    {
        /// <summary>
        /// Method to create new invites
        /// </summary>
        /// <param name="invite"></param>
        /// <returns></returns>
        public Task<Invite> CreateInviteAsync(Invite invite);

        /// <summary>
        /// Method to get an invite
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Task<Invite> GetInviteAsync(Guid inviteId);

        /// <summary>
        /// Method to get all invites for a user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<List<Invite>> GetUserInvitesAsync(Guid userId);

        /// <summary>
        /// Method to update an invite
        /// </summary>
        /// <param name="invite"></param>
        /// <returns></returns>
        public Task<Invite> UpdateInviteAsync(Invite invite);

        /// <summary>
        /// Method to get all invites for a business account
        /// </summary>
        /// <param name="businessAccountId"></param>
        /// <returns>All invites for a business account</returns>
        public Task<IEnumerable<Invite>> GetBusinessAccountInvitesAsync(Guid businessAccountId);

        /// <summary>
        /// Sends an email to the user inviting them to BUS
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<string> SendInviteEmailAsync(PostInviteRequest request);

        /// <summary>
        /// Verifies validity of tokens when sent
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<TokenVerificationResult> VerifyToken(string token);

        /// <summary>
        /// Cancels and sends a new invite to the installer
        /// </summary>
        /// <param name="inviteId"></param>
        /// <returns></returns>
        public Task<bool> ResendInviteAsync(Guid inviteId);

        /// <summary>
        /// Method to get all the invite statuses
        /// </summary>
        /// <returns>Collection of all invite status</returns>
        public Task<List<InviteStatus>> GetAllInviteStatusAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="govNotifyId"></param>
        /// <returns></returns>
        Task<string> CheckInviteEmailStatusAsync(string govNotifyId);


        /// <summary>
        /// Method to get all invites
        /// </summary>
        /// <returns>All invites</returns>
        public Task<IEnumerable<Invite>> GetAllInvitesAsync();
    }
}
