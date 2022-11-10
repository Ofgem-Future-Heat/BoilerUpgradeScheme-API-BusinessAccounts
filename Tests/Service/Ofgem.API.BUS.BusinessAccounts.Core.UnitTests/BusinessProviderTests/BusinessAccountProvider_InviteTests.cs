using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Ofgem.API.BUS.BusinessAccounts.Domain.Entities;
using Ofgem.API.BUS.BusinessAccounts.Domain.Exceptions;
using Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ofgem.API.BUS.Applications.Core.UnitTests.BusinessProviderTests;

public partial class BusinessAccountProviderTests : TestBaseWithSqlite
{
    [Test]
    public async Task CreateInviteAsync_NullInvite_Throws()
    {
        var result = async () => await _businessAccountProvider!.CreateInviteAsync(null!);

        await result.Should().ThrowAsync<BadRequestException>().WithMessage("No invite provided");
    }

    [Test]
    public async Task CreateInviteAsync_Invalid_Throws()
    {
        var result = async () => await _businessAccountProvider!.CreateInviteAsync(new());

        await result.Should().ThrowAsync<DbUpdateException>();
    }

    private Invite CreateValidTestInvite()
    {
        Guid externalUserAccountId = DbContext.ExternalUserAccounts.First().Id;
        Guid inviteStatusId = DbContext.InviteStatuses.First().Id;
        return new()
        {
            ID = Guid.NewGuid(),
            EmailAddress = "test@ofgem.gov.uk",
            ExpiresOn = DateTime.UtcNow,
            FullName = "Test User",
            AccountName = "Account Name",
            ExternalUserAccountId = externalUserAccountId,
            StatusID = inviteStatusId,
        };
    }

    [Test]
    public async Task GetAllInvites_Valid_ReturnsList()
    {
        Invite invite = CreateValidTestInvite();
        var created = await _businessAccountProvider!.CreateInviteAsync(invite);

        var result = await _businessAccountProvider!.GetAllInvitesAsync();

        result.Should().BeEquivalentTo(new List<Invite>() { created });  
    }

    [Test]
    public async Task CreateInviteAsync_InvalidEmail_Throws()
    {
        Invite invite = CreateValidTestInvite();
        invite.EmailAddress = null!;

        var result = async () => await _businessAccountProvider!.CreateInviteAsync(invite);

        await result.Should().ThrowAsync<DbUpdateException>();
    }

    [Test]
    public async Task CreateInviteAsync_InvalidFullName_Throws()
    {
        Invite invite = CreateValidTestInvite();
        invite.FullName = null!;

        var result = async () => await _businessAccountProvider!.CreateInviteAsync(invite);

        await result.Should().ThrowAsync<DbUpdateException>();
    }

    [Test]
    public async Task CreateInviteAsync_InvalidAccountName_Throws()
    {
        Invite invite = CreateValidTestInvite();
        invite.AccountName = null!;

        var result = async () => await _businessAccountProvider!.CreateInviteAsync(invite);

        await result.Should().ThrowAsync<DbUpdateException>();
    }

    [Test]
    public async Task CreateInviteAsync_InvalidStatusId_Throws()
    {
        Invite invite = CreateValidTestInvite();
        invite.StatusID = Guid.Empty;

        var result = async () => await _businessAccountProvider!.CreateInviteAsync(invite);

        await result.Should().ThrowAsync<DbUpdateException>();
    }

    [Test]
    public async Task CreateInviteAsync_InvalidUserId_Throws()
    {
        Invite invite = CreateValidTestInvite();
        invite.ExternalUserAccountId = Guid.Empty;

        var result = async () => await _businessAccountProvider!.CreateInviteAsync(invite);

        await result.Should().ThrowAsync<DbUpdateException>();
    }

    [Test]
    public async Task CreateInviteAsync_Valid_Creates()
    {
        Invite invite = CreateValidTestInvite();

        var result = await _businessAccountProvider!.CreateInviteAsync(invite);

        result.Should().BeEquivalentTo(invite);
    }

    [Test]
    public async Task GetInviteAsync_EmptyId_Throws()
    {
        var result = async () => await _businessAccountProvider!.GetInviteAsync(Guid.Empty);

        await result.Should().ThrowAsync<BadRequestException>().WithMessage("No GUID provided");
    }

    [Test]
    public async Task GetInviteAsync_InvalidId_Throws()
    {
        var result = async () => await _businessAccountProvider!.GetInviteAsync(Guid.NewGuid());

        await result.Should().ThrowAsync<BadRequestException>().WithMessage("Invite not found");
    }

    [Test]
    public async Task GetInviteAsync_Valid_Returns()
    {
        Invite invite = CreateValidTestInvite();
        var created = await _businessAccountProvider!.CreateInviteAsync(invite);

        var result = await _businessAccountProvider!.GetInviteAsync(created.ID);

        result.Should().BeEquivalentTo(invite);
    }

    [Test]
    public async Task GetUserInvitesAsync_EmptyId_Throws()
    {
        var result = async () => await _businessAccountProvider!.GetUserInvitesAsync(Guid.Empty);

        await result.Should().ThrowAsync<BadRequestException>().WithMessage("No user provided");
    }

    [Test]
    public async Task GetUserInvitesAsync_NoInvites_ReturnsEmptyList()
    {
        Guid externalUserAccountId = DbContext.ExternalUserAccounts.First().Id;
        var result = await _businessAccountProvider!.GetUserInvitesAsync(externalUserAccountId);

        result.Should().BeEquivalentTo(new List<Invite>());
    }

    [Test]
    public async Task GetUserInvitesAsync_HasInvites_ReturnsList()
    {
        Guid externalUserAccountId = DbContext.ExternalUserAccounts.First().Id; 
        Invite invite = CreateValidTestInvite();
        var created = await _businessAccountProvider!.CreateInviteAsync(invite);

        var result = await _businessAccountProvider!.GetUserInvitesAsync(externalUserAccountId);

        result.Should().BeEquivalentTo(new List<Invite>() { created });
    }

    [Test]
    public async Task GetBusinessAccountInvitesAsync_NoInvites_ReturnsEmptyList()
    {
        Guid businessAccountId = DbContext.ExternalUserAccounts.First().BusinessAccountID;

        var result = await _businessAccountProvider!.GetBusinessAccountInvitesAsync(businessAccountId);

        result.Should().BeEquivalentTo(new List<Invite>());
    }

    [Test]
    public async Task GetBusinessAccountInvitesAsync_HasInvites_ReturnsList()
    {
        Guid businessAccountId = DbContext.ExternalUserAccounts.First().BusinessAccountID;
        Invite invite = CreateValidTestInvite();
        var created = await _businessAccountProvider!.CreateInviteAsync(invite);

        var result = await _businessAccountProvider!.GetBusinessAccountInvitesAsync(businessAccountId);

        result.Should().BeEquivalentTo(new List<Invite>() { created });
    }

    [Test]
    public async Task UpdateInviteAsync_Valid_Updates()
    {
        Invite invite = CreateValidTestInvite();
        await _businessAccountProvider!.CreateInviteAsync(invite);
        invite.FullName = "Updated Name";

        var updated = await _businessAccountProvider!.UpdateInviteAsync(invite);
        var result = await _businessAccountProvider!.GetInviteAsync(updated.ID);

        result.FullName.Should().Be("Updated Name");
    }

    [Test]
    public async Task GetAllInviteStatusAsync_Valid_ReturnsList()
    {
        var result = await _businessAccountProvider!.GetAllInviteStatusAsync();

        result.Count.Should().BeGreaterThan(0);
    }

    [Test]
    public async Task GetAllInviteStatusAsync_None_Throws()
    {
        DbContext.InviteStatuses.RemoveRange(DbContext.InviteStatuses);
        DbContext.SaveChanges();
        var result = async () => await _businessAccountProvider!.GetAllInviteStatusAsync();

        await result.Should().ThrowAsync<BadRequestException>().WithMessage("No invite status found");
    }
}
