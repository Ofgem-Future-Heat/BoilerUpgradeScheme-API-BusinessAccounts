using Microsoft.AspNetCore.Mvc;
using Ofgem.API.BUS.BusinessAccounts.Api.Extensions;
using Ofgem.API.BUS.BusinessAccounts.Core.Interfaces;
using Ofgem.API.BUS.BusinessAccounts.Domain.Entities;
using Ofgem.API.BUS.BusinessAccounts.Domain.Exceptions;
using Ofgem.API.BUS.BusinessAccounts.Domain.Request;
using Ofgem.Lib.BUS.AuditLogging.Api.Filters;

namespace Ofgem.API.BUS.BusinessAccounts.Api;

/// <summary>
/// Controller for the business accounts API endpoints
/// </summary>
[ApiController]
[Route("[controller]")]
public class BusinessAccountsController : ControllerBase
{
    /// <summary>
    /// Accounts service for interaction with the database
    /// </summary>
    private readonly IAccountsService _accountsService;

    public BusinessAccountsController(IAccountsService accountsService)
    {
        _accountsService = accountsService ?? throw new ArgumentNullException(nameof(accountsService));
    }

    /// <summary>
    /// POST request to add a new business account and return the account Id
    /// </summary>
    /// <param name="postBusinessAccountRequest"></param>
    /// <returns>200 & Business account Id</returns>
    [Route("")]
    [HttpPost]
    [AuditLogFilterFactory(Message = "Create business account")]
    public async Task<IActionResult> AddBusinessAccount(PostBusinessAccountRequest postBusinessAccountRequest)
    {
        Guid businessAccountId;
        try
        {
            businessAccountId = await _accountsService.AddBusinessAccount(postBusinessAccountRequest);
            postBusinessAccountRequest.AuthorisedRepresentative.CreatedBy = postBusinessAccountRequest.CreatedBy;
            postBusinessAccountRequest.AuthorisedRepresentative.CreatedDate = DateTime.UtcNow;
            postBusinessAccountRequest.AuthorisedRepresentative.BusinessAccountID = businessAccountId;
            await _accountsService.UpdateBusinessAccountAuthorisedRepresentative(postBusinessAccountRequest.AuthorisedRepresentative, false);
            HttpContext.Items.Add(AuditLogAttribute.EntityIdHttpContextKey, businessAccountId);
        }
        catch (BadRequestException ex)
        {
            return this.AsObjectResult(ex);
        }
        return Ok(businessAccountId);
    }

    /// <summary>
    /// POST request to add a new set of users and link them to a business account
    /// </summary>
    /// <param name="businessAccountID"></param>
    /// <param name="postUserAccountRequest"></param>
    /// <returns></returns>
    /// <exception cref="BadRequestException"></exception>
    [Route("{businessAccountID}/Users")]
    [HttpPost]
    [AuditLogFilterFactory(Message = "Create user accounts")]
    public async Task<IActionResult> SetupUserAccounts(Guid businessAccountID, PostUserAccountRequest postUserAccountRequest)
    {
        try
        {
            await _accountsService.AddBusinessAccountUsers(postUserAccountRequest, businessAccountID);
        }
        catch (BadRequestException ex)
        {
            return this.AsObjectResult(ex);
        }
        return Ok();
    }

    /// <summary>
    /// GET request to get all users linked to a business account
    /// </summary>
    /// <param name="businessAccountID"></param>
    /// <returns>A List of <see cref="ExternalUserAccount"/> objects</returns>
    [Route("{businessAccountID}/Users")]
    [HttpGet]
    public async Task<IActionResult> GetUsersByBusinessAccountIdAsync(Guid businessAccountID)
    {
        List<ExternalUserAccount> users;
        try
        {
            users = await _accountsService.GetUsersByBusinessAccountIdAsync(businessAccountID);
        }
        catch (ResourceNotFoundException ex)
        {
            return this.AsObjectResult(ex);
        }

        return Ok(users);
    }


    /// <summary>
    /// Gets - Request of paginated business accounts.
    /// </summary>
    /// <param name="page">Page number.</param>
    /// <param name="pageSize">Page size - number of items per page.</param>
    /// <param name="sortBy">Column to sort by.</param>
    /// <param name="orderByDescending">orderByDescending - True/False - either sort descending or ascending.</param>
    /// <param name="filterBusinessAcountsStatusBy">List of business account status code selected to filter the data by.</param>
    /// <param name="searchBy">Search data for matching text value - business ID or reference number.</param>
    /// <returns>IList of data objects.</returns>
    [Route("GetPagedBusinessAccounts/{page}/{pageSize}/{sortBy}/{orderByDescending}/{filterBusinessAcountsStatusBy}/{searchBy?}")]
    [HttpGet]
    public async Task<IActionResult> GetPagedBusinessAccounts(int page = 1, int pageSize = 20,
        string sortBy = "AccountSetupRequestDate", bool orderByDescending = true, 
        string filterBusinessAcountsStatusBy = "", string searchBy = "")
    {
        try
        {
            return Ok(await _accountsService.GetPagedBusinessAccounts(page, pageSize, sortBy, orderByDescending, filterBusinessAcountsStatusBy, searchBy));
        }
        catch (BadRequestException ex)
        {
            return this.AsObjectResult(ex);
        }
    }

    /// <summary>
    /// Endpoint to allow get all business accounts
    /// </summary>
    /// <returns></returns>
    [Route("GetAllBusinessAccounts")]
    [HttpGet]
    public async Task<IActionResult> GetAllBusinessAccounts()
    {
        try
        {
            return Ok(await _accountsService.GetAllBusinessAccounts());
        }
        catch (BadRequestException ex)
        {
            return this.AsObjectResult(ex);
        }
    }


    /// <summary>
    /// Endpoint to allow specific business accounts to be returned.
    /// </summary>
    /// <param name="businessAccountId"></param>
    /// <returns></returns>
    [Route("{businessAccountId}")]
    [HttpGet]
    public async Task<IActionResult> GetBusinessAccountById(Guid businessAccountId)
    {
        try
        {
            return Ok(await _accountsService.GetBusinessAccountById(businessAccountId));
        }
        catch (BadRequestException ex)
        {
            return this.AsObjectResult(ex);
        }
    }

    /// <summary>
    /// Endpoint to allow specific business account to be updated.
    /// </summary>
    /// <param name="businessAccountId"></param>
    /// <param name="businessAccount"></param>
    /// <returns>The updated object</returns>
    [Route("{businessAccountId}")]
    [HttpPost]
    [AuditLogFilterFactory(Message = "Update business account")]
    public async Task<IActionResult> UpdateBusinessAccountByIdAsync(Guid businessAccountId, BusinessAccount businessAccount)
    {
        if (businessAccount is null)
        {
            throw new ArgumentNullException(nameof(businessAccount));
        }

        if (businessAccountId != businessAccount.Id)
        {
            throw new BadArgumentException("Business account IDs do not match");
        }

        try
        {
            var MCSCheck = await _accountsService.AccountWithMcsNumberExistsAsync(businessAccount.MCSCertificationNumber, businessAccountId);
            if (!MCSCheck)
            {
                return Ok(await _accountsService.UpdateBusinessAccountAsync(businessAccount));
            } else
            {
                throw new BadRequestException("An active account already exists with this MCS number");
            }
            
        }
        catch (BadRequestException ex)
        {
            return this.AsObjectResult(ex);
        }
    }


    /// <summary>
    /// Endpoint to allow specific business accounts to be returned with full details.
    /// </summary>
    /// <param name="businessAccountId">Selected business account ID.</param>
    /// <returns>Fully populated business account entity.</returns>
    [Route("{businessAccountId}/GetFullBusinessAccountById")]
    [HttpGet]
    public async Task<IActionResult> GetFullBusinessAccountById(Guid businessAccountId)
    {
        try
        {
            return Ok(await _accountsService.GetFullBusinessAccountById(businessAccountId));
        }
        catch (BadRequestException ex)
        {
            return this.AsObjectResult(ex);
        }
    }

    /// <summary>
    /// Endpoint to allow specific external business accounts to be returned with full details.
    /// </summary>
    /// <param name="externalUserAccountId">Selected external business account ID.</param>
    /// <returns>Fully populated external business account entity.</returns>
    [Route("GetExternalUserAccountById/{externalUserAccountId}")]
    [HttpGet]
    public async Task<IActionResult> GetExternalUserAccountById(Guid externalUserAccountId)
    {
        try
        {
            return Ok(await _accountsService.GetExternalUserAccountById(externalUserAccountId));
        }
        catch (BadRequestException ex)
        {
            return this.AsObjectResult(ex);
        }
    }

    /// <summary>
    /// POST method to update external user accounts for a particular business account
    /// </summary>
    /// <param name="businessAccountId"></param>
    /// <param name="externalUsers"></param>
    /// <returns></returns>
    [Route("UpdateUsers")]
    [HttpPost]
    [AuditLogFilterFactory(Message = "Update business account users")]
    public async Task<IActionResult> UpdateExternalUsers(List<ExternalUserAccount> externalUsers)
    {
        try
        {
            return Ok(await _accountsService.UpdateBusinessAccountUsers(externalUsers));
        }
        catch (BadRequestException ex)
        {
            return this.AsObjectResult(ex);
        }
    }

    /// <summary>
    /// POST method to update external user accounts for a particular business account
    /// </summary>
    /// <param name="businessAccountId"></param>
    /// <param name="authorisedReprasentative"></param>
    /// <returns></returns>
    [Route("UpdateAuthorisedRepresentative")]
    [HttpPost]
    [AuditLogFilterFactory(Message = "Update business account authorised reprasentative")]
    public async Task<IActionResult> UpdateAuthorisedReprasentative(ExternalUserAccount authorisedReprasentative)
    {
        try
        {
            return Ok(await _accountsService.UpdateBusinessAccountAuthorisedRepresentative(authorisedReprasentative));
        }
        catch (BadRequestException ex)
        {
            return this.AsObjectResult(ex);
        }
    }

    /// <summary>
    /// POST method to update an additional external business account user
    /// </summary>
    /// <param name="additionalUser"></param>
    /// <returns>The additional user account which was updated</returns>
    [Route("UpdateAdditionalUser")]
    [HttpPost]
    public async Task<IActionResult> UpdateAdditionalUser(ExternalUserAccount additionalUser)
    {
        try
        {
            return Ok(await _accountsService.UpdateBusinessAccountAdditionalUser(additionalUser));
        }
        catch (BadRequestException ex)
        {
            return this.AsObjectResult(ex);
        }
    }

    /// <summary>
    /// Endpoint to return an email address of business account from an ID
    /// </summary>
    /// <param name="businessAccountId"></param>
    /// <returns></returns>
    [Route("{businessAccountId}/BusinessEmail")]
    [HttpGet]
    public async Task<IActionResult> GetBusinessAccountEmailById(Guid businessAccountId)
    {
        try
        {
            return Ok(await _accountsService.GetBusinessAccountEmailById(businessAccountId));
        }
        catch (BadRequestException ex)
        {
            return this.AsObjectResult(ex);
        }
    }

    /// <summary>
    /// Endpoint to return an email address of Installer when given an ID
    /// </summary>
    /// <param name="businessAccountId"></param>
    /// <returns></returns>
    [Route("{installerId}/InstallerEmail")]
    [HttpGet]
    public async Task<IActionResult> GetInstallerEmailAddressByInstallerID(Guid installerId)
    {
        try
        {
            return Ok(await _accountsService.GetExternalUserEmailAddressByInstallerID(installerId));
        }
        catch (BadRequestException ex)
        {
            return this.AsObjectResult(ex);
        }
    }

    /// <summary>
    /// Retrieves all the business accounts statuses.
    /// </summary>
    /// <returns>List of Business Accounts Sub status.</returns>
    /// <exception cref="BadRequestException"></exception>
    [Route("GetBusinessSubStatusesAsync")]
    [HttpGet]
    public async Task<IEnumerable<BusinessAccountSubStatus>> GetBusinessSubStatusesAsync()
    {
        IEnumerable<BusinessAccountSubStatus>? appLitsOfStatuses = await _accountsService.GetBusinessAccountsSubStatusesListAsync();

        return !appLitsOfStatuses.Any()
            ? throw new BadRequestException("Could not find list of business acocount statuses")
            : appLitsOfStatuses;
    }

    /// <summary>
    /// Sets the status of the business account, and updates the last modified by data 
    /// </summary>
    /// <param name="request">The update request</param>
    [Route("{businessAccountId}")]
    [HttpPut]
    [AuditLogFilterFactory(Message = "Update business account status")]
    public async Task<IActionResult> UpdateBusinessAccountStatusOnlyAsync(Guid businessAccountId, [FromBody]UpdateBusinessAccountStatusRequest request)
    {
        await _accountsService.UpdateBusinessAccountStatusOnlyAsync(request);
        return Ok();
    }
}