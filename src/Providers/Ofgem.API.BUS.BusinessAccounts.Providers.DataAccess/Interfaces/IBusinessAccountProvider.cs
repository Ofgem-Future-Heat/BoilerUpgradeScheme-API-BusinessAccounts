using Ofgem.API.BUS.BusinessAccounts.Domain.Entities;
using Ofgem.API.BUS.BusinessAccounts.Domain.Request;
using static Ofgem.API.BUS.BusinessAccounts.Domain.Entities.BankAccountStatus;
using static Ofgem.API.BUS.BusinessAccounts.Domain.Entities.BusinessAccountSubStatus;

namespace Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess.Interfaces
{
    /// <summary>
    /// This interfaces the CRUD operations for the Business Accounts DB.
    /// </summary>
    public interface IBusinessAccountProvider
    {
        /// <summary>
        /// This function checks our Business accounts Database for a pre-existing account.
        /// </summary>
        /// <param name="mcsNumber"></param>
        /// <returns></returns>
        public Task<List<BusinessAccount>> GetBusinessAccountsForMcsNumber(string mcsNumber);

        /// <summary>
        /// This method adds the new business account to the database
        /// </summary>
        /// <param name="businessAccount"></param>
        public Task AddBusinessAccount(BusinessAccount businessAccount);

        /// <summary>
        /// Gets a SubStatus object via the SubStatusCode enum
        /// </summary>
        /// <returns></returns>
        public Task<BusinessAccountSubStatus> GetSubStatusAsync(BusinessAccountSubStatusCode subStatusCode);
        /// <summary>
        /// Gets a BankAccountStatus via the BankAccountStatusCode enum
        /// </summary>
        /// <param name="bankAccountStatus"></param>
        /// <returns></returns>
        public Task<BankAccountStatus> GetBankAccountStatusAsync(BankAccountStatusCode bankAccountStatusCode);

        /// <summary>
        /// This method loops through a list of external user accounts and adds them to the database
        /// </summary>
        /// <param name="externalUsers"></param>
        /// <param name="businessAccountId"></param>
        public Task AddBusinessAccountUsers(List<ExternalUserAccount> externalUsers, Guid businessAccountId);

        /// <summary>
        /// Get - Business Accounts with pagination 
        /// </summary>
        /// <param name="page">Page number giving the page to start at, default to 1.</param>
        /// <param name="pageSize">Page size number of items per page requested by the calling program.</param>
        /// <param name="sortBy">Column to sort by.</param>
        /// <param name="orderByDescending">.</param>
        /// <param name="filterBusinessAcountsStatusBy">List of business account status code selected to filter the data by.</param>
        /// <param name="searchBy">Refine the display by allowing the user to search for text strings.</param>
        /// <returns>Returns an IList of Business Accounts wrapped in a Paged result object.</returns>
        public Task<PagedResult<BusinessDashboard>> GetPagedBusinessAccounts(int page = 1, int pageSize = 20,
           string sortBy = "AccountSetupRequestDate", bool orderByDescending = true,
           List<string>? filterBusinessAcountsStatusBy = null, string searchBy = "");

        /// <summary>
        /// returns all business accounts
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<BusinessAccount>> GetAllBusinessAccounts();

        /// <summary>
        /// Returns an individual business account.
        /// </summary>
        /// <param name="businessAccountId"></param>
        /// <returns></returns>
        public Task<BusinessAccount> GetBusinessAccountById(Guid businessAccountId);

        /// <summary>
        /// Returns an individual business account - full details.
        /// </summary>
        /// <param name="businessAccountId">The desired business account Id.</param>
        /// <returns>Selected Business Account with FULL detils.</returns>
        public Task<BusinessAccount> GetFullBusinessAccountById(Guid businessAccountId);

        /// <summary>
        /// Returns an email address of business account from an ID
        /// </summary>
        /// <param name="businessAccountId"></param>
        /// <returns></returns>
        public Task<string> GetBusinessAccountAuthorisedRepresentativeEmailByIdAsync(Guid businessAccountId);

        /// <summary>
        /// Gets the email of the installer, who's ID is given.
        /// </summary>
        /// <param name="InstallerId"></param>
        /// <returns></returns>
        public Task<string> GetExternalUserEmailByInstallerId(Guid InstallerId);

        /// <summary>
        /// Gets all <see cref="ExternalUserAccount"/>s linked to a business account
        /// </summary>
        /// <param name="businessAccountID">The business account ID</param>
        /// <returns>aList of all <see cref="ExternalUserAccount"/>s linked to a business account</returns>
        public Task<List<ExternalUserAccount>> GetUsersByBusinessAccountIdAsync(Guid businessAccountID);

        /// <summary>
        /// Adds a new row to the <see cref="GlobalSetting>"/> table and returns the new NextBusinessAccountReferenceNumber
        /// </summary>
        /// <returns>A NextBusinessAccountReferenceNumber to use for BusinessAccount creation</returns>
        public Task<int> GetNewBusinessAccountNumberAsync();

        /// <summary>
        /// UpdateBusinessAccountAsync - update the business account by ID.
        /// </summary>
        /// <param name="businessAccount"></param>
        /// <returns>BusinessAccount entity populated from the DB.</returns>
        public Task<BusinessAccount> UpdateBusinessAccountAsync(BusinessAccount businessAccount);

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
        /// Checks and updates business account users
        /// </summary>
        /// <param name="externalUsers"></param>
        /// <returns></returns>
        public Task<List<ExternalUserAccount>> UpdateBusinessAccountUsers(List<ExternalUserAccount> externalUsers);

        /// <summary>
        /// Checks and updates business account an Authorised reprasentative.
        /// </summary>
        /// <param name="authorisedRepresentative"></param>
        /// <returns></returns>
        public Task<ExternalUserAccount> UpdateAuthorisedRepresentative(ExternalUserAccount authorisedRepresentative);

        /// <summary>
        /// method to create a new invite
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Invite> CreateInviteAsync(Invite invite);

        /// <summary>
        /// method to retrieve a specific invite
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Invite> GetInviteAsync(Guid id);

        /// <summary>
        /// Method to return all invites for a user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<List<Invite>> GetUserInvitesAsync(Guid userId);

        /// <summary>
        /// Gets all invites
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<Invite>> GetAllInvitesAsync();

        /// <summary>
        /// method to update an invite
        /// </summary>
        /// <param name="invite"></param>
        /// <returns></returns>
        public Task<Invite> UpdateInviteAsync(Invite invite);

        /// <summary>
        /// Method to return all invites for a buisiness account
        /// </summary>
        /// <param name="businessAccountID"></param>
        /// <returns>All invites for a business account</returns>
        public Task<IEnumerable<Invite>> GetBusinessAccountInvitesAsync(Guid businessAccountID);

        /// <summary>
        /// Updates an additonal user for a business account
        /// </summary>
        /// <param name="additionalUser"></param>
        /// <returns>The additional user account which was updated</returns>
        public Task<ExternalUserAccount> UpdateBusinessAccountUser(ExternalUserAccount additionalUser);

        /// <summary>
        /// Get all Invite statuses
        /// </summary>
        /// <returns>Collection of Invite status objects</returns>
        public Task<List<InviteStatus>> GetAllInviteStatusAsync();

        public Task<string> GetBusinessAccountNameForEmail(string emailAddress, Guid userId);
    }
}
