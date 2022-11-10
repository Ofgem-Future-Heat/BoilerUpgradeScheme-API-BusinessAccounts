using Ofgem.API.BUS.BusinessAccounts.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ofgem.API.BUS.BusinessAccounts.Core.Interfaces
{
    /// <summary>
    /// A Interface containing a collection of Mcs service Functions that act as operations relative to the overall BUS Solution. 
    /// </summary>
    public interface IMcsService
    {
        /// <summary>
        /// This function checks that a given Mcs Number is valid.
        /// </summary>
        /// <param name="mcsNumber"></param>
        public Task CheckMcsNumber(string mcsNumber);
    }
}
