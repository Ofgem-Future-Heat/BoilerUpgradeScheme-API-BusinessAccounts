using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ofgem.API.BUS.BusinessAccounts.Domain.Entities
{
    public class Invite
    {
        //ID for Invite
        public Guid ID { get; set; }
        //ID For external user
        public Guid ExternalUserAccountId { get; set; }
        //full name of the user
        public string FullName { get; set; }
        //Account name of the user
        public string AccountName { get; set; }
        //Email address for the user
        public string EmailAddress { get; set; }
        //Date invite was sent
        public DateTime SentOn { get; set; }
        //Date invite expires on
        public DateTime ExpiresOn { get; set; }
        //Foreign key for external user
        public virtual ExternalUserAccount? ExternalUserAccount { get; set; }
        /// <summary>
        /// ID of the current status
        /// </summary>
        public Guid StatusID { get; set; }
        /// <summary>
        /// Navigation property to the Invite status
        /// </summary>
        public virtual InviteStatus? Status { get; set; }
        public string? GovNotifyId { get; set; }
    }
}
