using AutoMapper;
using Ofgem.API.BUS.BusinessAccounts.Core.Interfaces;
using Ofgem.API.BUS.BusinessAccounts.Domain.Constants;
using Ofgem.API.BUS.BusinessAccounts.Domain.Entities;
using Ofgem.API.BUS.BusinessAccounts.Domain.Exceptions;
using Ofgem.API.BUS.BusinessAccounts.Domain.Request;
using Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess.Interfaces;
using System.Data;
using static Ofgem.API.BUS.BusinessAccounts.Domain.Entities.BusinessAccountSubStatus;

namespace Ofgem.API.BUS.BusinessAccounts.Core;

public class BusinessAccountsService : IAccountsService
{
    /// <summary>
    /// Interface for allowing transactions with the database.
    /// </summary>
    private readonly IBusinessAccountProvider _businessAccountProvider;

    /// <summary>
    /// Auto-mapper interface for mapping post requests to types.
    /// </summary>
    private readonly IMapper _mapper;

    public BusinessAccountsService(IMapper mapper, IBusinessAccountProvider businessAccountProvider)
    {
        _mapper = mapper;
        _businessAccountProvider = businessAccountProvider ?? throw new ArgumentNullException(nameof(businessAccountProvider));
    }

    /// <summary>
    /// AccountWithMcsNumberExistsAsync - Checks if an account with the given MCS number exists in a Live business account
    /// </summary>
    /// <param name="mcsNumber">The MCS number to be checked.</param>
    /// <param name="allowedAccountId">The ID of one 'allowed' Business Account with the given MCS number</param>
    /// <returns>Whether an account with the MCS number was found</returns>
    public async Task<Boolean> AccountWithMcsNumberExistsAsync(string mcsNumber, Guid? allowedAccountId = null)
    {
        if (mcsNumber is null)
        {
            throw new BadRequestException(AccountsExceptionMessages.NoAccountProvided);
        }

        allowedAccountId ??= Guid.Empty;

        // Gets a list of all accounts with the mcs number, if any returned, return true, otherwise return false as no account exists
        var linkedAccounts = await _businessAccountProvider.GetBusinessAccountsForMcsNumber(mcsNumber);
        if (linkedAccounts.Count > 0)
        {
            foreach (var account in linkedAccounts.Where(x => x.Id != allowedAccountId))
            {
                if (account.SubStatusId != StatusMappings.BusinessAccountSubStatus[BusinessAccountSubStatus.BusinessAccountSubStatusCode.FAIL].Id
                    && account.SubStatusId != StatusMappings.BusinessAccountSubStatus[BusinessAccountSubStatus.BusinessAccountSubStatusCode.WITHDR].Id
                    && account.SubStatusId != StatusMappings.BusinessAccountSubStatus[BusinessAccountSubStatus.BusinessAccountSubStatusCode.REVOK].Id)
                {
                    return true;
                }
            }
            return false;
        }
        return false;
    }

    /// <summary>
    /// This method triggers the MCS account check and provided thats false then adds the account to the database
    /// </summary>
    /// <param name="postBusinessAccountRequest"></param>
    /// <returns>Business account Id.</returns>
    public async Task<Guid> AddBusinessAccount(PostBusinessAccountRequest postBusinessAccountRequest)
    {
        await CheckForDuplicateEmail(postBusinessAccountRequest.AuthorisedRepresentative.EmailAddress, postBusinessAccountRequest.AuthorisedRepresentative.Id);

        var mappedBusinessAccount = _mapper.Map<BusinessAccount>(postBusinessAccountRequest);
        bool mcsNumberInUse = await AccountWithMcsNumberExistsAsync(mappedBusinessAccount.MCSCertificationNumber);

        if (!mcsNumberInUse)
        {
            // populate required, programmatically generated fields
            mappedBusinessAccount = await AddNewBusinessAccountReferenceNumberAsync(mappedBusinessAccount);
            mappedBusinessAccount.SubStatus = await _businessAccountProvider.GetSubStatusAsync(BusinessAccountSubStatus.BusinessAccountSubStatusCode.SUBMIT);
            mappedBusinessAccount.SubStatusId = mappedBusinessAccount.SubStatus.Id;
            if (mappedBusinessAccount.BankAccounts != null)
            {
                foreach (var bankAccount in mappedBusinessAccount.BankAccounts)
                {
                    bankAccount.Status = await _businessAccountProvider.GetBankAccountStatusAsync(BankAccountStatus.BankAccountStatusCode.ACTIVE);
                    bankAccount.StatusID = bankAccount.Status.Id;
                    bankAccount.SunAccountNumber = mappedBusinessAccount.BusinessAccountNumber;
                }
            }

            await _businessAccountProvider.AddBusinessAccount(mappedBusinessAccount);
            return mappedBusinessAccount.Id;
        }
        else
        {
            throw new BadRequestException(AccountsExceptionMessages.McsNumberInUseError);
        }
    }

    /// <summary>
    /// This method adds the list of external user accounts from the post request into the database.
    /// </summary>
    /// <param name="postUserAccountRequest"></param>
    /// <param name="businessAccountId"></param>
    public async Task AddBusinessAccountUsers(PostUserAccountRequest postUserAccountRequest, Guid businessAccountId)
    {
        var mappedBusinessAccountUsers = _mapper.Map<List<ExternalUserAccount>>(postUserAccountRequest);

        await _businessAccountProvider.AddBusinessAccountUsers(mappedBusinessAccountUsers, businessAccountId);
    }

    /// <summary>
    /// Paginate through the business accounts.
    /// </summary>
    /// <param name="page">Page number giving the page to start at, default to 1.</param>
    /// <param name="pageSize">Page size number of items per page requested by the calling program.</param>
    /// <param name="sortBy">Column to sort by.</param>
    /// <param name="orderByDescending">.</param>
    /// <param name="filterBusinessAcountsStatusBy">List of business account status code selected to filter the data by.</param>
    /// <param name="searchBy">Refine the display by allowing the user to search for text strings.</param>
    /// <returns>Returns an IList of Business Accounts formatted into a BusinessDashboard object and wrapped in a PagedResult object.</returns>
    public async Task<PagedResult<BusinessDashboard>> GetPagedBusinessAccounts(int page = 1, int pageSize = 20, string sortBy = "AccountSetupRequestDate", bool orderByDescending = true,
        string filterBusinessAcountsStatusBy = "", string searchBy = "")
    {
        /* Parse the application statuses passed into the operation*/
        List<string>? listOfFilterBusinessAccountStatusBy = null;
        if (!string.IsNullOrEmpty(filterBusinessAcountsStatusBy))
        {
            listOfFilterBusinessAccountStatusBy = filterBusinessAcountsStatusBy.Replace(@"\", "").Split(",").ToList();
        }

        /* Stop the search and degrade gracefully */
        if (!string.IsNullOrEmpty(searchBy) && searchBy.Replace(@"\", "").Length < 3)
        {
            searchBy = string.Empty;
        }

        return await _businessAccountProvider.GetPagedBusinessAccounts(page, pageSize, sortBy, orderByDescending, listOfFilterBusinessAccountStatusBy, searchBy);
    }

    /// <summary>
    /// GetAllBusinessAccounts - get all business accounts
    /// </summary>
    /// <returns>Returns a list business accounts</returns>
    public async Task<IEnumerable<BusinessAccount>> GetAllBusinessAccounts()
    {
        return await _businessAccountProvider.GetAllBusinessAccounts();
    }


    /// <summary>
    /// GetBusinessAccountById - Returns an individual business account by Id.
    /// </summary>
    /// <param name="businessAccountId">The passed in business Id to search by.</param>
    /// <returns>Returns an individual business account by Id.</returns>
    public async Task<BusinessAccount> GetBusinessAccountById(Guid businessAccountId)
    {
        if (businessAccountId == Guid.Empty)
        {
            throw new BadRequestException(AccountsExceptionMessages.NoGuidError);
        }
        return await _businessAccountProvider.GetBusinessAccountById(businessAccountId);
    }

    /// <summary>
    /// GetFullBusinessAccountById - Returns an individual business account by Id with the full details.
    /// </summary>
    /// <param name="businessAccountId">The passed in business Id to search by.</param>
    /// <returns>Returns an individual business account by Id with the full details.</returns>
    public async Task<BusinessAccount> GetFullBusinessAccountById(Guid businessAccountId)
    {
        if (businessAccountId == Guid.Empty)
        {
            throw new BadRequestException(AccountsExceptionMessages.NoGuidError);
        }
        return await _businessAccountProvider.GetFullBusinessAccountById(businessAccountId);
    }

    /// <summary>
    /// Returns an email address of business account from an ID
    /// </summary>
    /// <param name="businessAccountId"></param>
    /// <returns></returns>
    public async Task<string> GetBusinessAccountEmailById(Guid businessAccountId)
    {
        if (businessAccountId == Guid.Empty)
        {
            throw new BadRequestException(AccountsExceptionMessages.NoGuidError);
        }
        return await _businessAccountProvider.GetBusinessAccountAuthorisedRepresentativeEmailByIdAsync(businessAccountId);
    }

    public async Task<string> GetExternalUserEmailAddressByInstallerID(Guid installerId)
    {
        if (installerId == Guid.Empty)
        {
            throw new BadRequestException(AccountsExceptionMessages.NoGuidError);
        }
        return await _businessAccountProvider.GetExternalUserEmailByInstallerId(installerId);
    }

    public async Task<List<ExternalUserAccount>> GetUsersByBusinessAccountIdAsync(Guid businessAccountId)
    {
        return await _businessAccountProvider.GetUsersByBusinessAccountIdAsync(businessAccountId);
    }

    private async Task<BusinessAccount> AddNewBusinessAccountReferenceNumberAsync(BusinessAccount businessAccount)
    {
        var nextBusinessAccountReferenceNumber = await _businessAccountProvider.GetNewBusinessAccountNumberAsync();
        businessAccount.BusinessAccountNumber = $"BUS{nextBusinessAccountReferenceNumber}";
        return businessAccount;
    }

    public async Task<BusinessAccount> UpdateBusinessAccountAsync(BusinessAccount businessAccount)
    {
        return await _businessAccountProvider.UpdateBusinessAccountAsync(businessAccount);
    }

    public async Task<ExternalUserAccount> GetExternalUserAccountById(Guid externalUserAccountId)
    {
        return await _businessAccountProvider.GetExternalUserAccountById(externalUserAccountId);
    }

    public async Task<IEnumerable<BusinessAccountSubStatus>> GetBusinessAccountsSubStatusesListAsync()
    {
        return await _businessAccountProvider.GetBusinessAccountsSubStatusesListAsync();
    }

    public async Task UpdateBusinessAccountStatusOnlyAsync(UpdateBusinessAccountStatusRequest request)
    {
        await _businessAccountProvider.UpdateBusinessAccountStatusOnlyAsync(request);
    }

    public async Task<List<ExternalUserAccount>> UpdateBusinessAccountUsers(List<ExternalUserAccount> externalUsers)
    {
        return await _businessAccountProvider.UpdateBusinessAccountUsers(externalUsers);
    }
    public async Task<ExternalUserAccount> UpdateBusinessAccountAuthorisedRepresentative(ExternalUserAccount authorisedRepresentative, bool checkForDuplicates = true)
    {
        if (checkForDuplicates)
        {
            await CheckForDuplicateEmail(authorisedRepresentative.EmailAddress, authorisedRepresentative.Id);
        }
        return await _businessAccountProvider.UpdateAuthorisedRepresentative(authorisedRepresentative);
    }

    public async Task<ExternalUserAccount> UpdateBusinessAccountAdditionalUser(ExternalUserAccount additionalUser)
    {
        await CheckForDuplicateEmail(additionalUser.EmailAddress, additionalUser.Id);
        return await _businessAccountProvider.UpdateBusinessAccountUser(additionalUser);
    }

    private async Task CheckForDuplicateEmail(string emailAddress, Guid userId)
    {
        string businessAccountName = await _businessAccountProvider.GetBusinessAccountNameForEmail(emailAddress, userId);
        if (!string.IsNullOrEmpty(businessAccountName))
        {
            throw new BadRequestException(AccountsExceptionMessages.EmailAddressInUse.Replace("[businessName]", businessAccountName));
        }
    }
}