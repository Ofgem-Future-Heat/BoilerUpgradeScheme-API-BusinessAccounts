using Microsoft.AspNetCore.Mvc;
using Ofgem.API.BUS.BusinessAccounts.Api.Extensions;
using Ofgem.API.BUS.BusinessAccounts.Core.Interfaces;
using Ofgem.API.BUS.BusinessAccounts.Domain.Constants;
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
public class ExternalBusinessAccountsController : ControllerBase
{
    /// <summary>
    /// Accounts service for interaction with the database
    /// </summary>
    private readonly IAccountsService _accountsService;

    public ExternalBusinessAccountsController(IAccountsService accountsService)
    {
        _accountsService = accountsService ?? throw new ArgumentNullException(nameof(accountsService));
    }

    /* PENDING - test to reflect security/Authentication/identity when decision has been implemented */

    /// <summary>
    /// Endpoint to allow specific business accounts to be returned with full details.
    /// </summary>
    /// <param name="businessAccountId">Selected business account ID.</param>
    /// <returns>Fully populated business account entity.</returns>
    [Route("ExternalGetBusinessAccountById/{businessAccountId}")]
    [HttpGet]
    public async Task<IActionResult> ExternalGetBusinessAccountById(Guid? businessAccountId)
    {
        try
        {
            if (businessAccountId == null || businessAccountId == Guid.Empty)
            {
                throw new BadRequestException(AccountsExceptionMessages.NoGuidError);
            }

            return Ok(await _accountsService.GetFullBusinessAccountById((Guid)businessAccountId));
        }
        catch (BadRequestException ex)
        {
            return this.AsObjectResult(ex);
        }
    }

    /// <summary>
    /// GET request to get all users linked to a business account.
    /// </summary>
    /// <param name="businessAccountID">Business Account for application.</param>
    /// <returns>A List of <see cref="ExternalUserAccount"/> objects.</returns>
    [Route("Users/{businessAccountId}")]
    [HttpGet]
    public async Task<IActionResult> ExternalGetUsersByBusinessAccountIdAsync(Guid businessAccountId)
    {
        List<ExternalUserAccount> users;
        try
        {
            if (businessAccountId == Guid.Empty)
            {
                throw new BadRequestException(AccountsExceptionMessages.NoGuidError);
            }

            users = await _accountsService.GetUsersByBusinessAccountIdAsync(businessAccountId);
        }
        catch (BadRequestException ex)
        {
            return this.AsObjectResult(ex);
        }

        return Ok(users);
    }

    /// <summary>
    /// GET request to get external business account user.
    /// </summary>
    /// <param name="externalUserAccountId">External business account user ID.</param>
    /// <returns>Fully populated external business account user entity.</returns>
    [Route("ExternalGetUserAccountById/{externalUserAccountId}")]
    [HttpGet]
    public async Task<IActionResult> GetExternalUserAccountById(Guid externalUserAccountId)
    {
        try
        {
            if (externalUserAccountId == Guid.Empty)
            {
                throw new BadRequestException(AccountsExceptionMessages.NoGuidError);
            }

            return Ok(await _accountsService.GetExternalUserAccountById(externalUserAccountId));
        }
        catch (BadRequestException ex)
        {
            return this.AsObjectResult(ex);
        }
    }

    /// <summary>
    /// POST method to update external user accounts
    /// </summary>
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
}