using AutoMapper;
using Ofgem.API.BUS.BusinessAccounts.Domain.Entities;
using Ofgem.API.BUS.BusinessAccounts.Domain.Request;

namespace Ofgem.API.BUS.BusinessAccounts.Core.Automapper.Converters
{
    public  class BusinessAccountUserConverter : ITypeConverter<PostUserAccountRequest, List<ExternalUserAccount>>
    {
        /// <summary>
        /// This method converts the postUseraccountrequest into a list of external user accounts for adding to the Database.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public List<ExternalUserAccount> Convert(PostUserAccountRequest source, List<ExternalUserAccount> destination, ResolutionContext context)
        {
            return source.ExternalUserAccounts
                .Select(userAccountRequest => new ExternalUserAccount
                {
                    CreatedBy = source.CreatedBy,
                    BusinessAccountID = source.BusinessAccountID,
                    AADB2CId = userAccountRequest.AADB2CId,
                    AuthorisedRepresentative = userAccountRequest.AuthorisedRepresentative,
                    EmailAddress = userAccountRequest.EmailAddress,
                    CoHoRoleID = userAccountRequest.CoHoRoleID,
                    HomeAddress = userAccountRequest.HomeAddress,
                    HomeAddressPostcode = userAccountRequest.HomeAddressPostcode,
                    HomeAddressUPRN = userAccountRequest.HomeAddressUPRN,
                    DOB = userAccountRequest.DOB,
                    FirstName = userAccountRequest.FirstName,
                    LastName = userAccountRequest.LastName,
                    RoleId = userAccountRequest.RoleId,
                    StandardUser = userAccountRequest.StandardUser,
                    SuperUser = userAccountRequest.SuperUser,
                    TelephoneNumber = userAccountRequest.TelephoneNumber
                })
                .ToList();
        }
    }
}
