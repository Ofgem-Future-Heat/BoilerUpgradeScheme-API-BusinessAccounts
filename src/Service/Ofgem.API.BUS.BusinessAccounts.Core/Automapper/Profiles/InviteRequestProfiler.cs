using AutoMapper;
using Ofgem.API.BUS.BusinessAccounts.Core.Automapper.Converters;
using Ofgem.API.BUS.BusinessAccounts.Domain.Entities;
using Ofgem.API.BUS.BusinessAccounts.Domain.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ofgem.API.BUS.BusinessAccounts.Core.Automapper.Profiles
{
    public  class InviteRequestProfiler : Profile
    {
        public InviteRequestProfiler()
        {
            CreateMap<PostInviteRequest, Invite>()
                .ConvertUsing<InviteConverter>();
        }
    }
}
