using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ofgem.API.BUS.BusinessAccounts.Domain.CommsObjects
{
    public class TokenVerificationResult
    {
        public bool TokenAccepted { get; set; }

        public Guid InviteID { get; set; }

        public DateTime InviteTokenExpiryDate { get; set; }
    }
}
