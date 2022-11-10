using Microsoft.AspNetCore.Mvc;
using Ofgem.API.BUS.BusinessAccounts.Api.Extensions;
using Ofgem.API.BUS.BusinessAccounts.Core.Interfaces;
using Ofgem.API.BUS.BusinessAccounts.Domain.Entities;
using Ofgem.API.BUS.BusinessAccounts.Domain.Exceptions;
using Ofgem.API.BUS.BusinessAccounts.Domain.Request;
using Ofgem.Lib.BUS.AuditLogging.Api.Filters;
using System.Text;

namespace Ofgem.API.BUS.BusinessAccounts.Api;
/// <summary>
/// Controller for the invite API endpoints
/// </summary>
[ApiController]
[Route("[controller]")]
public class InviteController : ControllerBase
{
    /// <summary>
    /// Service for interacting with the DB for invites
    /// </summary>
    private readonly IInviteService _inviteService;

    private readonly IAccountsService _accountsService;

    public InviteController(IInviteService inviteService, IAccountsService accountsService)
    {
        _inviteService = inviteService ?? throw new ArgumentNullException(nameof(inviteService));
        _accountsService = accountsService ?? throw new ArgumentNullException(nameof(accountsService));
    }

    /// <summary>
    /// POST method to create a new invite
    /// </summary>
    /// <param name="invite"></param>
    /// <returns></returns>
    [Route("")]
    [HttpPost]
    public async Task<IActionResult> CreateInviteAsync(PostInviteRequest request)
    {
        Invite newInvite;
        try
        {
            var foundUserAccount = await _accountsService.GetExternalUserAccountById(request.ExternalUserAccountId);
            var foundAccount = await _accountsService.GetBusinessAccountById(foundUserAccount.BusinessAccountID);

            newInvite = await _inviteService.CreateInviteAsync(request.Invite);

            var sendInviteRequest = new PostInviteRequest()
            {
                Invite = newInvite,
                InviteId = newInvite.ID,
                BusinessAccount = foundAccount,
                BusinessAccountId = foundAccount.Id,
                ExternalUserAccount = foundUserAccount,
                ExternalUserAccountId = foundAccount.Id,
                InviteRequestExpiryDays = 7,
            };
            request.Invite.GovNotifyId = await _inviteService.SendInviteEmailAsync(sendInviteRequest);
            await _inviteService.UpdateInviteAsync(newInvite);
        }
        catch (BadRequestException ex)
        {
            return this.AsObjectResult(ex);
        }
        return Ok(newInvite.ID);
    }

    /// <summary>
    /// GET Method to return a specific invite
    /// </summary>
    /// <param name="inviteId"></param>
    /// <returns></returns>
    [Route("{inviteId}")]
    [HttpGet]
    public async Task<IActionResult> GetInviteAsync(Guid inviteId)
    {
        Invite foundInvite;
        try
        {
            foundInvite = await _inviteService.GetInviteAsync(inviteId);
            HttpContext.Items.Add(AuditLogAttribute.EntityIdHttpContextKey, foundInvite.ID);
        }
        catch (BadRequestException ex)
        {
            return this.AsObjectResult(ex);
        }
        return Ok(foundInvite);
    }

    /// <summary>
    /// GET Method to return all invites for a specific user
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [Route("User/{userId}")]
    [HttpGet]
    public async Task<IActionResult> GetInvitesForUserAsync(Guid userId)
    {
        List<Invite> foundInvites;
        try
        {
            foundInvites = await _inviteService.GetUserInvitesAsync(userId);
        }
        catch (BadRequestException ex)
        {
            return this.AsObjectResult(ex);
        }
        return Ok(foundInvites);
    }

    /// <summary>
    /// PUT method to update an invite
    /// </summary>
    /// <param name="inviteId"></param>
    /// <param name="invite"></param>
    /// <returns></returns>
    [Route("Update/{inviteId}")]
    [HttpPut]
    public async Task<IActionResult> UpdateInviteAsync(Guid inviteId, Invite invite)
    {
        Invite updatedInvite;
        try
        {
            updatedInvite = await _inviteService.UpdateInviteAsync(invite);
            HttpContext.Items.Add(AuditLogAttribute.EntityIdHttpContextKey, invite);
        }
        catch (BadRequestException ex)
        {
            return this.AsObjectResult(ex);
        }
        return Ok(updatedInvite);
    }

    /// <summary>
    /// GET Method to return all invites for a specific business account
    /// </summary>
    /// <param name="businessAccountId"></param>
    /// <returns></returns>
    [Route("BusinessAccount/{businessAccountId}")]
    [HttpGet]
    public async Task<IActionResult> GetInvitesForBusinessAccountAsync(Guid businessAccountId) =>
        Ok(await _inviteService.GetBusinessAccountInvitesAsync(businessAccountId));

    [Route("verify")]
    [HttpGet]
    public async Task<IActionResult> VerifyTokenAsync(string token)
    {
        try
        {
            var validatedToken = await _inviteService.VerifyToken(token);
            return Ok(validatedToken);
        }
        catch (BadRequestException ex)
        {
            return this.AsObjectResult(ex);
        }
    }

    [Route("{inviteId}/resend")]
    [HttpPost]
    public async Task<IActionResult> ResendInvite(Guid inviteId)
    {
        bool resendSuccess;
        try
        {
            resendSuccess = await _inviteService.ResendInviteAsync(inviteId);
        }
        catch (BadRequestException ex)
        {
            return this.AsObjectResult(ex);
        }
        return Ok(resendSuccess);
    }

    [Route("GetAllInviteStatus")]
    [HttpGet]
    public async Task<IActionResult> GetAllInviteStatusAsync()
    {
        List<InviteStatus> inviteStatus;
        try
        {
            inviteStatus = await _inviteService.GetAllInviteStatusAsync();
        }
        catch (BadRequestException ex)
        {
            return this.AsObjectResult(ex);
        }
        return Ok(inviteStatus);
    }

    [Route("GetInviteEmailStatusAsync/{govNotifyId}")]
    [HttpGet]
    public async Task<IActionResult> GetInviteEmailStatusAsync(string govNotifyId)
    {
        try
        {
            var status = await _inviteService.CheckInviteEmailStatusAsync(govNotifyId);
            return Ok(status);
        }
        catch (BadRequestException ex)
        {
            return this.AsObjectResult(ex);
        }
    }

    [Route("GetAllInvitesAsync")]
    [HttpGet]
    public async Task<IActionResult> GetAllInvitesAsync()
    {
        try
        {
            var invites = await _inviteService.GetAllInvitesAsync();
            return Ok(invites);
        }
        catch (BadRequestException ex)
        {
            return this.AsObjectResult(ex);
        }
    }

}
