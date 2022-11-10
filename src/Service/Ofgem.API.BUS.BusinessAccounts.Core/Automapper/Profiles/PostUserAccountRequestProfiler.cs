using AutoMapper;
using Ofgem.API.BUS.BusinessAccounts.Core.Automapper.Converters;
using Ofgem.API.BUS.BusinessAccounts.Domain.Entities;
using Ofgem.API.BUS.BusinessAccounts.Domain.Request;

namespace Ofgem.API.BUS.BusinessAccounts.Core.Automapper.Profiles;

public class PostUserAccountRequestProfiler : Profile
{
    /// <summary>
    /// This method sets up the Automapper profile to convert from the postuseraccountrequest type to a 
    /// list of external user accounts
    /// </summary>
    public PostUserAccountRequestProfiler()
    {
        CreateMap<PostUserAccountRequest, List<ExternalUserAccount>>()
            .ConvertUsing<BusinessAccountUserConverter>();
    }
}
