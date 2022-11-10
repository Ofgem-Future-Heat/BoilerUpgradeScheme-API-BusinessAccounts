using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ofgem.API.BUS.BusinessAccounts.Core.Models
{
    public class JobResultBusinessModel
    {
        public int? BatchSize { get; init; }
        public bool Success { get; }

        public JobResultBusinessModel(bool success = true)
        {
            Success = success;
        }
    }
}
