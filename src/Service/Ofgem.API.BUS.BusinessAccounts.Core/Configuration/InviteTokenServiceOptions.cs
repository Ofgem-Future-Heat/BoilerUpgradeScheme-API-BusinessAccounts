using Ofgem.API.BUS.BusinessAccounts.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ofgem.API.BUS.BusinessAccounts.Core
{
    public  class InviteTokenServiceOptions : IInviteTokenServiceOptions
    {
        private readonly string _tokenEmailTemplateId;
        private readonly string _inviteTokenSecret;
        private readonly string _externalPortalURL;
        private readonly string _installerReplyToAddress;

        public InviteTokenServiceOptions(string tokenEmailTemplateId, string inviteTokenSecret, string externalPortalURL, string installerReplyToAddress)
        {
            _tokenEmailTemplateId = tokenEmailTemplateId ?? throw new ArgumentNullException(nameof(tokenEmailTemplateId));
            _inviteTokenSecret = inviteTokenSecret ?? throw new ArgumentNullException(nameof(inviteTokenSecret));
            _externalPortalURL= externalPortalURL ?? throw new ArgumentNullException(nameof(externalPortalURL));
            _installerReplyToAddress = installerReplyToAddress ?? throw new ArgumentNullException(nameof(installerReplyToAddress));
        }

        public string TokenEmailtemplateId => _tokenEmailTemplateId;

        public string InviteTokenSecret => _inviteTokenSecret;

        public string ExternalPortalURL => _externalPortalURL;
        public string InstallerReplyToAddress => _installerReplyToAddress;
    }
}
