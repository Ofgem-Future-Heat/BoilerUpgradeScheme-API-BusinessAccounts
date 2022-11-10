using AutoMapper;
using Ofgem.API.BUS.BusinessAccounts.Domain.Entities;
using Ofgem.API.BUS.BusinessAccounts.Domain.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ofgem.API.BUS.BusinessAccounts.Core.Automapper.Converters
{
    public class InviteConverter : ITypeConverter<PostInviteRequest, Invite>
    {
        public Invite Convert (PostInviteRequest source, Invite destination, ResolutionContext context)
        {
            var invite = new Invite
            {
                ID = Guid.NewGuid(),
                ExternalUserAccountId = source.Invite.ExternalUserAccountId,
                FullName = source.Invite.FullName,
                AccountName = source.Invite.AccountName,
                SentOn = DateTime.Now,
                ExpiresOn = DateTime.Now.AddDays(source.InviteRequestExpiryDays),
                ExternalUserAccount = source.Invite.ExternalUserAccount
            };

            return invite;
        }
    }
}
