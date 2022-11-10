using Ofgem.API.BUS.BusinessAccounts.Domain.Entities;
using Ofgem.API.BUS.BusinessAccounts.Domain.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ofgem.API.BUS.BusinessAccounts.Core.Interfaces
{
    public interface IAccountsService
    {
        /// <summary>
        /// AccountWithMcsNumberExistsAsync - Checks if an account with the given MCS number exists in a Live business account
        /// </summary>
        /// <param name="mcsNumber">The MCS number to be checked.</param>
        /// <param name="allowedAccountId">The ID of one 'allowed' Business Account with the given MCS number</param>
        /// <returns>Whether an account with the MCS number was found</returns>
        public Task<Boolean> AccountWithMcsNumberExistsAsync(string mcsNumber, Guid? allowedAccountId = null);

        /// <summary>
        /// This method Adds a new business account to the database
        /// </summary>
        /// <param name="postBusinessAccountRequest"></param>
        /// <returns></returns>
        public Task<Guid> AddBusinessAccount(PostBusinessAccountRequest postBusinessAccountRequest);

        /// <summary>
        /// this method adds a list of user accounts to the database linked to a business account
        /// </summary>
        /// <param name="postUserAccountRequest"></param>
        /// <param name="businessAccountID"></param>
        /// <returns></returns>
        public Task AddBusinessAccountUsers(PostUserAccountRequest postUserAccountRequest, Guid businessAccountId);

        /// <summary>
        /// Get - Business Accounts with pagination 
        /// </summary>
        /// <param name="page">Page number giving the page to start at, default to 1.</param>
        /// <param name="pageSize">Page size number of items per page requested by the calling program.</param>
        /// <param name="sortBy">Column to sort by.</param>
        /// <param name="orderByDescending">.</param>
        /// <param name="filterBusinessAcountsStatusBy">List of business account status code selected to filter the data by.</param>
        /// <param name="searchBy">Refine the display by allowing the user to search for text strings.</param>
        /// <returns>Returns an IList of Business Accounts formatted into a BusinessDashboard object and wrapped in a Paged result object.</returns>
        Task<PagedResult<BusinessDashboard>> GetPagedBusinessAccounts(int page = 1, int pageSize = 20,
            string sortBy = "AccountSetupRequestDate", bool orderByDescending = true,
             string filterBusinessAcountsStatusBy = "", string searchBy = "");

        /// <summary>
        /// Gets all business accounts
        /// </summary>
        /// <returns>IEnumerable list of business accounts</returns>
        Task<IEnumerable<BusinessAccount>> GetAllBusinessAccounts();

        /// <summary>
        /// Returns an individual business account by its ID
        /// </summary>
        /// <returns></returns>
        public Task<BusinessAccount> GetBusinessAccountById(Guid businessAccountId);

        /// <summary>
        /// Returns an individual business account by its ID with full details.
        /// </summary>
        /// <returns>Business account entity fully populated.</returns>
        public Task<BusinessAccount> GetFullBusinessAccountById(Guid businessAccountId);

        /// <summary>
        /// Returns an email address of business account from an ID
        /// </summary>
        /// <param name="businessAccountId"></param>
        /// <returns></returns>
        public Task<string> GetBusinessAccountEmailById(Guid businessAccountId);

        /// <summary>
        /// Returns an Installer email address from an Installer ID
        /// </summary>
        /// <param name="installerId"></param>
        /// <returns></returns>
        public Task<string> GetExternalUserEmailAddressByInstallerID(Guid installerId);


        /// <summary>
        /// Gets all <see cref="ExternalUserAccount"/>s linked to a business account
        /// </summary>
        /// <param name="businessAccountID">The business account ID</param>
        /// <returns>A List of all <see cref="ExternalUserAccount"/>s linked to a business account</returns>
        Task<List<ExternalUserAccount>> GetUsersByBusinessAccountIdAsync(Guid businessAccountId);

        /// <summary>
        /// Updates the business account object if the updatedDate matches
        /// </summary>
        /// <param name="businessAccount"></param>
        /// <returns>The updated <see cref="BusinessAccount"/></returns>
        Task<BusinessAccount> UpdateBusinessAccountAsync(BusinessAccount businessAccount);

        /// <summary>
        /// GetExternalUserAccountById - Get the external user account details.
        /// </summary>
        /// <param name="externalUserAccountId">Table ID of the external ua ser account.</param>
        /// <returns>ExternalUserAccount entity populated from the DB.</returns>
        public Task<ExternalUserAccount> GetExternalUserAccountById(Guid externalUserAccountId);

        /// <summary>
        /// Retrieves all the business accounts statuses.
        /// </summary>
        /// <returns>List of Business Accounts Sub status.</returns>
        public Task<IEnumerable<BusinessAccountSubStatus>> GetBusinessAccountsSubStatusesListAsync();

        /// <summary>
        /// Sets the status of the business account, and updates the last modified by data 
        /// </summary>
        /// <param name="request">The update request</param>
        public Task UpdateBusinessAccountStatusOnlyAsync(UpdateBusinessAccountStatusRequest request);

        /// <summary>
        /// Updates the users for a business account
        /// </summary>
        /// <param name="externalUsers"></param>
        /// <returns></returns>
        public Task<List<ExternalUserAccount>> UpdateBusinessAccountUsers(List<ExternalUserAccount> externalUsers);

        /// <summary>
        /// Updates the users for a business account authorised representative.
        /// </summary>
        /// <param name="externalUsers"></param>
        /// <returns></returns>
        public Task<ExternalUserAccount> UpdateBusinessAccountAuthorisedRepresentative(ExternalUserAccount authorisedRepresentative, bool checkForDuplicates = true);


        /// <summary>
        /// Updates an additonal user for a business account
        /// </summary>
        /// <param name="additionalUser"></param>
        /// <returns>The additional user account which was updated</returns>
        public Task<ExternalUserAccount> UpdateBusinessAccountAdditionalUser(ExternalUserAccount additionalUser);
    }

}
