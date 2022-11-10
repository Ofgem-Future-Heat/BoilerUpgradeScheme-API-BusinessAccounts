using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ofgem.API.BUS.BusinessAccounts.Core.Interfaces
{
    public interface IInviteTokenServiceOptions
    {
        /// <summary>
        /// returns the template ID for the token email
        /// </summary>
        public string TokenEmailtemplateId { get; }

        /// <summary>
        /// returns the secret for the invite token
        /// </summary>
        public string InviteTokenSecret { get; }

        /// <summary>
        /// returns the url for the external portal
        /// </summary>
        public string ExternalPortalURL { get; }

        public string InstallerReplyToAddress { get; }
    }
}
