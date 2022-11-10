using AutoMapper;
using Ofgem.API.BUS.BusinessAccounts.Core.Automapper.Converters;
using Ofgem.API.BUS.BusinessAccounts.Domain.Entities;
using Ofgem.API.BUS.BusinessAccounts.Domain.Request;

namespace Ofgem.API.BUS.BusinessAccounts.Core.Automapper.Profiles;

public class PostBusinessAccountRequestProfiler : Profile
{
    /// <summary>
    /// This method sets up the profiler for Automapper and creates the map for converting the post business
    /// request to the business account type
    /// </summary>
    public PostBusinessAccountRequestProfiler()
    {
        CreateMap<PostBusinessAccountRequest, BusinessAccount>()
            .ConvertUsing<BusinessAccountConverter>();
    }
}
