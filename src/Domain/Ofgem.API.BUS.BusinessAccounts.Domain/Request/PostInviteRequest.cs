using Ofgem.API.BUS.BusinessAccounts.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ofgem.API.BUS.BusinessAccounts.Domain.Request
{
    public class PostInviteRequest
    {
        public int InviteRequestExpiryDays { get; set; }

        public Guid InviteId { get; set; }

        public Guid ExternalUserAccountId { get; set; }

        public Guid BusinessAccountId { get; set; }

        public virtual ExternalUserAccount ExternalUserAccount { get; set; }

        public virtual BusinessAccount BusinessAccount { get; set; }

        public virtual Invite Invite { get; set; }
    }
}
