using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Ofgem.API.BUS.BusinessAccounts.Domain.Constants;
using Ofgem.API.BUS.BusinessAccounts.Domain.Entities;
using Ofgem.API.BUS.BusinessAccounts.Domain.Exceptions;
using Ofgem.API.BUS.BusinessAccounts.Domain.Request;
using Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess.Helpers;
using Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess.Interfaces;
using static Ofgem.API.BUS.BusinessAccounts.Domain.Entities.AddressType;
using static Ofgem.API.BUS.BusinessAccounts.Domain.Entities.BankAccountStatus;
using static Ofgem.API.BUS.BusinessAccounts.Domain.Entities.BusinessAccountSubStatus;

namespace Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess;

public class BusinessAccountProvider : IBusinessAccountProvider
{
    private readonly BusinessAccountsDbContext _context;

    public BusinessAccountProvider(BusinessAccountsDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<List<BusinessAccount>> GetBusinessAccountsForMcsNumber(string mcsNumber)
    {
        var mcsNumberToBeChecked = mcsNumber.ToString();

        var mcsNumberfromDb = await _context.BusinessAccounts.Where(x => x.MCSCertificationNumber == mcsNumberToBeChecked)
            .ToListAsync().ConfigureAwait(false);

        return mcsNumberfromDb;
    }

    public async Task AddBusinessAccount(BusinessAccount businessAccount)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync().ConfigureAwait(false);
        _context.BusinessAccounts.Add(businessAccount);
        await _context.SaveChangesAsync().ConfigureAwait(false);
        await transaction.CommitAsync().ConfigureAwait(false);
    }

    public async Task<BusinessAccountSubStatus> GetSubStatusAsync(BusinessAccountSubStatusCode subStatusCode)
    {
        return await _context.BusinessAccountSubStatuses.SingleAsync(x => x.Code == subStatusCode);
    }

    public async Task<AddressType> GetAddressTypeAsync(AddressTypeCode addressTypeCode)
    {
        return await _context.AddressTypes.SingleAsync(x => x.Code.Equals(addressTypeCode));
    }

    public async Task<BankAccountStatus> GetBankAccountStatusAsync(BankAccountStatusCode bankAccountStatusCode)
    {
        return await _context.BankAccountStatuses.SingleAsync(x => x.Code == bankAccountStatusCode);
    }

    public async Task AddBusinessAccountUsers(List<ExternalUserAccount> externalUsers, Guid businessAccountId)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync().ConfigureAwait(false);
        foreach (var EUA in externalUsers)
        {
            EUA.BusinessAccountID = businessAccountId;
            _context.Add(EUA);
        }
        await _context.SaveChangesAsync().ConfigureAwait(false);
        await transaction.CommitAsync().ConfigureAwait(false);
    }

    private static void GetPagedBusinessAccountsParamCheck(string sortBy)
    {
        if (string.IsNullOrEmpty(sortBy))
        {
            throw new ArgumentException($"'{nameof(sortBy)}' cannot be null or empty.", nameof(sortBy));
        }
    }

    public async Task<PagedResult<BusinessDashboard>> GetPagedBusinessAccounts(int page = 1, int pageSize = 20, string sortBy = "AccountSetupRequestDate", bool orderByDescending = true,
        List<string>? filterBusinessAcountsStatusBy = null, string searchBy = "")
    {
        GetPagedBusinessAccountsParamCheck(sortBy);

        /* Do we have any data to display */
        IQueryable<BusinessDashboard>? businessAccounts = _context.BusinessDashboards.AsQueryable();
        if (!businessAccounts.Any())
        {
            return new();
        }

        /* Filtering */
        businessAccounts = FilterByStatusCode(filterBusinessAcountsStatusBy, businessAccounts);

        /* Search By string */
        businessAccounts = BusinessAccountSearchBy(searchBy ?? "", businessAccounts);

        /* Order data set by Ascending or Descending */
        IOrderedQueryable<BusinessDashboard> sortedDataSubSet = BusinessAccountsSortBy(sortBy, orderByDescending, businessAccounts);

        /* Return paged results ... */
        return await sortedDataSubSet.GetPagedAsync(page, pageSize).ConfigureAwait(false);
    }

    private static IOrderedQueryable<BusinessDashboard> BusinessAccountsSortBy(string sortBy, bool orderByDescending, IQueryable<BusinessDashboard> businessAccounts)
    {
        return sortBy switch
        {
            "BusinessName" => orderByDescending
               ? businessAccounts.OrderByDescending(x => x.BusinessName)
               : businessAccounts.OrderBy(x => x.BusinessName),

            "BusinessAccountNumber" => orderByDescending
               ? businessAccounts.OrderByDescending(x => x.BusinessAccountNumber)
               : businessAccounts.OrderBy(x => x.BusinessAccountNumber),

            "SubStatus" => orderByDescending
               ? businessAccounts.OrderByDescending(x => x.BusinessSubStatus)
               : businessAccounts.OrderBy(x => x.BusinessSubStatus),

            "ReviewRecommendation" => orderByDescending
               ? businessAccounts.OrderByDescending(x => x.ReviewRecommendation)
               : businessAccounts.OrderBy(x => x.ReviewRecommendation),

            "Amendment" => orderByDescending
               ? businessAccounts.OrderByDescending(x => x.BeingAmended)
               : businessAccounts.OrderBy(x => x.BeingAmended),

            "AccountSetupRequestDate" => orderByDescending
               ? businessAccounts.OrderByDescending(x => x.AccountSetupRequestDate)
               : businessAccounts.OrderBy(x => x.AccountSetupRequestDate),

            _ => businessAccounts.OrderByDescending(x => x.AccountSetupRequestDate),

        };
    }

    private static IQueryable<BusinessDashboard> BusinessAccountSearchBy(string searchBy, IQueryable<BusinessDashboard> businessAccounts)
    {
        if (!string.IsNullOrEmpty(searchBy) && searchBy.Length >= 3)
        {
            businessAccounts = businessAccounts.Where(c => c.BusinessName!.Contains(searchBy) || c.BusinessAccountNumber!.Contains(searchBy));
        }

        return businessAccounts;
    }

    private static IQueryable<BusinessDashboard> FilterByStatusCode(List<string>? filterBusinessAcountsStatusBy, IQueryable<BusinessDashboard> businessAccounts)
    {
        if (filterBusinessAcountsStatusBy != null)
        {
            return businessAccounts.Where(a => filterBusinessAcountsStatusBy.Contains(a.BusinessSubStatusCode!));
        }

        return businessAccounts;
    }

    public async Task<BusinessAccount> GetFullBusinessAccountById(Guid businessAccountId)
    {
        var businessAccount = await _context.BusinessAccounts
            .Include(b => b.SubStatus!.BusinessAccountStatus)
            .Include(b => b.CompanyType)
            .Include(b => b.BankAccounts)
            .Include(b => b.BusinessAddresses)
            .Include(b => b.CompaniesHouseDetails)
            .FirstOrDefaultAsync(x => x.Id == businessAccountId).ConfigureAwait(false);

        if (businessAccount == null)
        {
            throw new BadRequestException(AccountsExceptionMessages.NoAccountFound);
        }
        else
        {
            return businessAccount;
        }
    }
    public async Task<ExternalUserAccount> GetExternalUserAccountById(Guid externalUserAccountId)
    {
        var externalUserAccount = await _context.ExternalUserAccounts
            .FirstOrDefaultAsync(x => x.Id == externalUserAccountId).ConfigureAwait(false);

        if (externalUserAccount == null)
        {
            throw new BadRequestException(AccountsExceptionMessages.NoAccountFound);
        }
        else
        {
            return externalUserAccount;
        }
    }

    public async Task<IEnumerable<BusinessAccount>> GetAllBusinessAccounts()
    {
        var businessAccounts = await _context.BusinessAccounts.ToListAsync();

        if (businessAccounts.Count == 0)
        {
            throw new BadRequestException(AccountsExceptionMessages.NoAccountFound);
        }
        else
        {
            return businessAccounts;
        }
    }

    public async Task<BusinessAccount> GetBusinessAccountById(Guid businessAccountId)
    {
        var businessAccount = await _context.BusinessAccounts
            .Include("SubStatus.BusinessAccountStatus")
            .FirstOrDefaultAsync(x => x.Id == businessAccountId).ConfigureAwait(false);

        if (businessAccount == null)
        {
            throw new BadRequestException(AccountsExceptionMessages.NoAccountFound);
        }
        else
        {
            return businessAccount;
        }
    }

    public async Task<string> GetBusinessAccountAuthorisedRepresentativeEmailByIdAsync(Guid businessAccountId)
    {
        var authorisedRepresentative = await _context.ExternalUserAccounts
            .SingleOrDefaultAsync(x => x.BusinessAccountID == businessAccountId && x.AuthorisedRepresentative)
            .ConfigureAwait(false);

        return authorisedRepresentative?.EmailAddress ??
            throw new BadRequestException(AccountsExceptionMessages.NoAuthorisedRepresentativeFound);
    }

    public async Task<string> GetExternalUserEmailByInstallerId(Guid InstallerId)
    {
        var authorisedRepresentative = await _context.ExternalUserAccounts
            .SingleOrDefaultAsync(x => x.Id == InstallerId)
            .ConfigureAwait(false);

        return authorisedRepresentative?.EmailAddress ??
            throw new BadRequestException(AccountsExceptionMessages.NoAuthorisedRepresentativeFound);
    }

    public async Task<List<ExternalUserAccount>> GetUsersByBusinessAccountIdAsync(Guid businessAccountID)
    {
        return await _context.ExternalUserAccounts
            .Where(x => x.BusinessAccountID == businessAccountID)
            .ToListAsync()
            .ConfigureAwait(false);
    }

    public async Task<int> GetNewBusinessAccountNumberAsync()
    {
        var globalSetting = new GlobalSetting();
        var generatedById = Guid.NewGuid();
        globalSetting.GeneratedByID = generatedById;

        await using var transaction = await _context.Database.BeginTransactionAsync();
        _context.GlobalSettings.Add(globalSetting);
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return globalSetting.NextBusinessAccountReferenceNumber;
    }

    public async Task<BusinessAccount> UpdateBusinessAccountOldAsync(BusinessAccount businessAccount)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        var dbObject = await _context.BusinessAccounts
            .Include(x => x.BankAccounts)
            .Include(x => x.BusinessAddresses)
            .Include(x => x.CompaniesHouseDetails)
            .Include(x => x.CompanyType)
            .SingleOrDefaultAsync(x => x.Id == businessAccount.Id);
        if (dbObject == null)
        {
            throw new ResourceNotFoundException(AccountsExceptionMessages.NoAccountFound);
        }

        PopulateBusinessAccountPrimaryObject(businessAccount, dbObject);
        PopulateBankAccount(businessAccount, dbObject);
        PopulateBusinessAddress(businessAccount, dbObject);
        PopulateTradingAddress(businessAccount, dbObject);
        PopulateCompanyNumber(businessAccount, dbObject);

        _context.Entry(dbObject).State = EntityState.Modified;

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return dbObject;
    }

    private void PopulateCompanyNumber(BusinessAccount businessAccount, BusinessAccount dbObject)
    {
        var dbCompaniesHouseDetails = dbObject.CompaniesHouseDetails;
        var incomingCompaniesHouseDetails = businessAccount.CompaniesHouseDetails;

        if (incomingCompaniesHouseDetails is null)
        {
            if (dbCompaniesHouseDetails is not null)
            {
                _context.CompaniesHouseDetails.Remove(dbCompaniesHouseDetails);
                dbObject.CoHoID = null;
                _context.Entry(dbObject).State = EntityState.Modified;
            }
        }
        else
        {
            if (dbCompaniesHouseDetails is null)
            {
                incomingCompaniesHouseDetails.Id = Guid.NewGuid();
                incomingCompaniesHouseDetails.CreatedBy = String.Empty;
                dbObject.CompaniesHouseDetails = incomingCompaniesHouseDetails;
                dbObject.CoHoID = incomingCompaniesHouseDetails.Id;
            }
            else
            {
                dbCompaniesHouseDetails.CompanyNumber = incomingCompaniesHouseDetails.CompanyNumber;
                _context.Entry(dbCompaniesHouseDetails).State = EntityState.Modified;
            }
        }
    }

    private void PopulateTradingAddress(BusinessAccount businessAccount, BusinessAccount dbObject)
    {
        var dbTradingAddress = dbObject.BusinessAddresses!.FirstOrDefault(x => x.AddressTypeId == TypeMappings.AddressType[AddressTypeCode.TRADE].Id);
        var incomingTradingAddress = businessAccount.BusinessAddresses!.FirstOrDefault(x => x.AddressTypeId == TypeMappings.AddressType[AddressTypeCode.TRADE].Id);

        if (incomingTradingAddress is null)
        {
            if (dbTradingAddress is not null)
            {
                _context.BusinessAddresses.Remove(dbTradingAddress);
            }
        }
        else
        {
            if (dbTradingAddress is null)
            {
                dbObject.BusinessAddresses!.Add(incomingTradingAddress);
            }
            else
            {
                dbTradingAddress.AddressLine1 = incomingTradingAddress.AddressLine1;
                dbTradingAddress.AddressLine2 = incomingTradingAddress.AddressLine2;
                dbTradingAddress.AddressLine3 = incomingTradingAddress.AddressLine3;
                dbTradingAddress.AddressLine4 = incomingTradingAddress.AddressLine4;
                dbTradingAddress.County = incomingTradingAddress.County;
                dbTradingAddress.Postcode = incomingTradingAddress.Postcode;
                dbTradingAddress.UPRN = incomingTradingAddress.UPRN;
                _context.Entry(dbTradingAddress).State = EntityState.Modified;
            }
        }
    }

    private static void PopulateBusinessAddress(BusinessAccount businessAccount, BusinessAccount dbObject)
    {
        var dbBusinessAddress = dbObject.BusinessAddresses!.First(x => x.AddressTypeId == TypeMappings.AddressType[AddressTypeCode.BIZ].Id);
        var incomingBusinessAddress = businessAccount.BusinessAddresses!.First(x => x.AddressTypeId == TypeMappings.AddressType[AddressTypeCode.BIZ].Id);
        dbBusinessAddress.AddressLine1 = incomingBusinessAddress.AddressLine1;
        dbBusinessAddress.AddressLine2 = incomingBusinessAddress.AddressLine2;
        dbBusinessAddress.AddressLine3 = incomingBusinessAddress.AddressLine3;
        dbBusinessAddress.AddressLine4 = incomingBusinessAddress.AddressLine4;
        dbBusinessAddress.County = incomingBusinessAddress.County;
        dbBusinessAddress.Postcode = incomingBusinessAddress.Postcode;
        dbBusinessAddress.UPRN = incomingBusinessAddress.UPRN;
        dbBusinessAddress.LastUpdatedBy = incomingBusinessAddress.LastUpdatedBy;
        dbBusinessAddress.LastUpdatedDate = incomingBusinessAddress.LastUpdatedDate;
    }

    private static void PopulateBankAccount(BusinessAccount businessAccount, BusinessAccount dbObject)
    {
        var dbBankAccount = dbObject.BankAccounts!.First(x => x.StatusID == StatusMappings.BankAccountStatus[BankAccountStatusCode.ACTIVE].Id);
        var incomingBankAccount = businessAccount.BankAccounts!.First();
        dbBankAccount.AccountNumber = incomingBankAccount.AccountNumber;
        dbBankAccount.SortCode = incomingBankAccount.SortCode;
        dbBankAccount.AccountName = incomingBankAccount.AccountName;
        dbBankAccount.LastUpdatedBy = incomingBankAccount.LastUpdatedBy;
        dbBankAccount.LastUpdatedDate = incomingBankAccount.LastUpdatedDate;
    }

    private static void PopulateBusinessAccountPrimaryObject(BusinessAccount businessAccount, BusinessAccount dbObject)
    {
        dbObject.SubStatusId = businessAccount.SubStatusId;
        dbObject.CompanyTypeId = businessAccount.CompanyTypeId;
        dbObject.BusinessName = businessAccount.BusinessName;
        dbObject.TradingName = businessAccount.TradingName;
        dbObject.MCSCertificationNumber = businessAccount.MCSCertificationNumber;
        dbObject.MCSCertificationBody = businessAccount.MCSCertificationBody;
        dbObject.MCSMembershipNumber = businessAccount.MCSMembershipNumber;
        dbObject.MCSConsumerCode = businessAccount.MCSConsumerCode;
        dbObject.MCSCompanyType = businessAccount.MCSCompanyType;
        dbObject.MCSId = businessAccount.MCSId;
        dbObject.MCSAddressID = businessAccount.MCSAddressID;
        dbObject.AccountSetupRequestDate = businessAccount.AccountSetupRequestDate;
        dbObject.BusinessAccountNumber = businessAccount.BusinessAccountNumber;
        dbObject.MCSContactDetailsID = businessAccount.MCSContactDetailsID;
        dbObject.ActiveDate = businessAccount.ActiveDate;
        dbObject.IsUnderInvestigation = businessAccount.IsUnderInvestigation;
        dbObject.DARecommendation = businessAccount.DARecommendation;
        dbObject.LastUpdatedBy = businessAccount.LastUpdatedBy;
        dbObject.LastUpdatedDate = businessAccount.LastUpdatedDate;
        dbObject.QCRecommendation = businessAccount.QCRecommendation;

        if (businessAccount.SubStatusId == StatusMappings.BusinessAccountSubStatus[BusinessAccountSubStatusCode.ACTIV].Id &&
            businessAccount.ActiveDate is null)
        {
            dbObject.ActiveDate = DateTime.UtcNow;
        }
    }

    public async Task<IEnumerable<BusinessAccountSubStatus>> GetBusinessAccountsSubStatusesListAsync()
    {
        return await _context.BusinessAccountSubStatuses.ToListAsync().ConfigureAwait(false);
    }

    public async Task UpdateBusinessAccountStatusOnlyAsync(UpdateBusinessAccountStatusRequest request)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        var businessAccount = await _context.BusinessAccounts.FindAsync(request.BusinessAccountId);

        if (businessAccount == null)
        {
            throw new ResourceNotFoundException($"Business account with ID {request.BusinessAccountId} not found.");
        }
    }

    public async Task<BusinessAccount> UpdateBusinessAccountAsync(BusinessAccount businessAccount)
    {
        await using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync().ConfigureAwait(false);

        BusinessAccount? foundAccount = await _context.BusinessAccounts.FindAsync(businessAccount.Id).ConfigureAwait(false);

        if (foundAccount == null)
        {
            throw new ResourceNotFoundException($"Business account not found for {businessAccount.Id}");
        }

        if (businessAccount?.BusinessAddresses?.Any() ?? false)
        {
            foreach (var address in businessAccount.BusinessAddresses)
            {
                BusinessAddress? foundAddress = await _context.BusinessAddresses.FindAsync(address.Id);
                if (foundAddress == null)
                {
                    Guid id = _context.BusinessAddresses.Add(address).Entity.Id;
                    address.Id = id;
                }
                else
                {
                    _context.Entry(foundAddress).CurrentValues.SetValues(address);
                }
            }
        }

        if (businessAccount?.BankAccounts?.Any() ?? false)
        {
            foreach (var bankAccount in businessAccount.BankAccounts)
            {
                BankAccount? foundBankAccount = await _context.BankAccounts.FindAsync(bankAccount.Id);
                if (foundBankAccount == null)
                {
                    Guid id = _context.BankAccounts.Add(bankAccount).Entity.Id;
                    bankAccount.Id = id;
                }
                else
                {
                    _context.Entry(foundBankAccount).CurrentValues.SetValues(bankAccount);
                }
            }
        }

        if (businessAccount?.CompaniesHouseDetails != null)
        {
            CompaniesHouseDetail? foundCoHoDetails = await _context.CompaniesHouseDetails.FindAsync(businessAccount.CompaniesHouseDetails.Id);
            if (foundCoHoDetails == null)
            {
                businessAccount.CompaniesHouseDetails.CreatedDate = DateTime.UtcNow;
                businessAccount.CompaniesHouseDetails.CreatedBy = businessAccount.CreatedBy;
                Guid id = _context.CompaniesHouseDetails.Add(businessAccount.CompaniesHouseDetails).Entity.Id;
                businessAccount.CompaniesHouseDetails.Id = id;
            }
            else
            {
                _context.Entry(foundCoHoDetails).CurrentValues.SetValues(businessAccount.CompaniesHouseDetails);
            }
        }

        // EXPIRE PENDING INVITES
        var statusIdsWhichShouldCauseAllExternalUsersInvitesToBeCancelled = new[]
        {
            StatusMappings.BusinessAccountSubStatus[BusinessAccountSubStatusCode.FAIL].Id,
            StatusMappings.BusinessAccountSubStatus[BusinessAccountSubStatusCode.REVOK].Id,
            StatusMappings.BusinessAccountSubStatus[BusinessAccountSubStatusCode.WITHDR].Id,
            StatusMappings.BusinessAccountSubStatus[BusinessAccountSubStatusCode.SUSPEND].Id,
        };

        if (statusIdsWhichShouldCauseAllExternalUsersInvitesToBeCancelled.Contains(businessAccount!.SubStatusId))
        {
            var businessAccountInvites = await GetBusinessAccountInvitesAsync(businessAccount.Id)
                .ConfigureAwait(false);

            var latestInvites = businessAccountInvites
                .Where(x => x.StatusID == StatusMappings.InviteStatus[InviteStatus.InviteStatusCode.INVITED].Id)
                .GroupBy(i => i.ExternalUserAccountId)
                .ToDictionary(
                    g => g.Key,
                    g => g.OrderBy(x => x.SentOn)
                            .Last()
                );

            foreach (var invite in latestInvites.Values)
            {
                invite.ExpiresOn = DateTime.UtcNow;
                invite.StatusID = StatusMappings.InviteStatus[InviteStatus.InviteStatusCode.CANCELLED].Id;
            }
        }
        // END EXPIRE PENDING INVITES

        _context.Entry(foundAccount).CurrentValues.SetValues(businessAccount);
        await _context.SaveChangesAsync().ConfigureAwait(false);
        await transaction.CommitAsync().ConfigureAwait(false);

        return businessAccount;
    }

    public async Task<List<ExternalUserAccount>> UpdateBusinessAccountUsers(List<ExternalUserAccount> externalUsers)
    {
        await using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync().ConfigureAwait(false);

        foreach (var user in externalUsers)
        {
            var loadedUser = await _context.ExternalUserAccounts.FindAsync(user.Id);

            if (loadedUser == null)
            {
                Guid id = _context.ExternalUserAccounts.Add(user).Entity.Id;
                user.Id = id;
            }
            else
            {
                _context.Entry(loadedUser).CurrentValues.SetValues(user);
            }
        }

        await _context.SaveChangesAsync().ConfigureAwait(false);
        await transaction.CommitAsync().ConfigureAwait(false);
        return externalUsers;
    }

    public async Task<ExternalUserAccount> UpdateAuthorisedRepresentative(ExternalUserAccount authorisedRepresentative)
    {
        await using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync().ConfigureAwait(false);

        var loadedUser = await _context.ExternalUserAccounts.FindAsync(authorisedRepresentative.Id);

        if (loadedUser == null)
        {
            Guid id = _context.ExternalUserAccounts.Add(authorisedRepresentative).Entity.Id;
            authorisedRepresentative.Id = id;
        }
        else
        {
            _context.Entry(loadedUser).CurrentValues.SetValues(authorisedRepresentative);
        }
        await _context.SaveChangesAsync().ConfigureAwait(false);
        await transaction.CommitAsync().ConfigureAwait(false);
        return authorisedRepresentative;
    }

    public async Task<Invite> CreateInviteAsync(Invite invite)
    {
        await using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync();

        if (invite == null)
        {
            throw new BadRequestException("No invite provided");
        }

        Guid id = _context.Invites.Add(invite).Entity.ID;
        invite.ID = id;

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
        return invite;
    }

    public async Task<Invite> GetInviteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new BadRequestException("No GUID provided");
        }

        var foundInvite = await _context.Invites.Include(x => x.ExternalUserAccount).Where(x => x.ID == id).ToListAsync();

        if (foundInvite.Count == 0)
        {
            throw new BadRequestException("Invite not found");
        }

        return foundInvite.First();
    }

    public async Task<List<Invite>> GetUserInvitesAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new BadRequestException("No user provided");
        }

        var invites = await _context.Invites
                        .Include(x => x.ExternalUserAccount)
                        .Include(x => x.Status)
                        .Where(i => i.ExternalUserAccountId == userId).ToListAsync();

        if (!invites.Any())
        {
            return new List<Invite>();
        }

        return invites;
    }

    public async Task<Invite> UpdateInviteAsync(Invite invite)
    {
        await using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync().ConfigureAwait(false);

        var loadedInvite = await _context.Invites.FindAsync(invite.ID);

        if (loadedInvite == null)
        {
            Guid id = _context.Invites.Add(invite).Entity.ID;
            invite.ID = id;
        }
        else
        {
            _context.Entry(loadedInvite).CurrentValues.SetValues(invite);
        }
        await _context.SaveChangesAsync().ConfigureAwait(false);
        await transaction.CommitAsync().ConfigureAwait(false);
        return invite;
    }

    public async Task<IEnumerable<Invite>> GetBusinessAccountInvitesAsync(Guid businessAccountID) =>
        await _context.Invites.Where(i => i.ExternalUserAccount != null && i.ExternalUserAccount.BusinessAccountID == businessAccountID)
        .Include(i => i.Status)
        .ToListAsync();

    public async Task<ExternalUserAccount> UpdateBusinessAccountUser(ExternalUserAccount additionalUser)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync().ConfigureAwait(false);
        var loadedUser = await _context.ExternalUserAccounts.FindAsync(additionalUser.Id);
        if (loadedUser == null)
        {
            Guid id = _context.ExternalUserAccounts.Add(additionalUser).Entity.Id;
            additionalUser.Id = id;
        }
        else
        {
            _context.Entry(loadedUser).CurrentValues.SetValues(additionalUser);
        }
        await _context.SaveChangesAsync().ConfigureAwait(false);
        await transaction.CommitAsync().ConfigureAwait(false);
        return additionalUser;
    }

    public async Task<List<InviteStatus>> GetAllInviteStatusAsync()
    {
        var inviteStatuses = await _context.InviteStatuses.ToListAsync();

        if (inviteStatuses.Count == 0)
        {
            throw new BadRequestException("No invite status found");
        }
        else
        {
            return inviteStatuses;
        }
    }

    public async Task<string> GetBusinessAccountNameForEmail(string emailAddress, Guid userId)
    {
        var externalUserAccount = await _context.ExternalUserAccounts
            .Where(x => x.EmailAddress == emailAddress && x.Id != userId)
            .FirstOrDefaultAsync().ConfigureAwait(false);
        if (externalUserAccount != null)
        {
            return (await GetBusinessAccountById(externalUserAccount.BusinessAccountID))?.BusinessName ?? "";
        }
        return "";
    }

    public async Task<IEnumerable<Invite>> GetAllInvitesAsync()
    {
        return await _context.Invites.ToListAsync();
    }
}
