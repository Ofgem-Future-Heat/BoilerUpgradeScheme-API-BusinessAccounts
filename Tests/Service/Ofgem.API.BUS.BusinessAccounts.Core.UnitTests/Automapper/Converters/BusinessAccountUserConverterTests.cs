using AutoMapper;
using FluentAssertions;
using NUnit.Framework;
using Ofgem.API.BUS.BusinessAccounts.Core.Automapper.Converters;
using Ofgem.API.BUS.BusinessAccounts.Core.Automapper.Profiles;
using Ofgem.API.BUS.BusinessAccounts.Domain.Entities;
using Ofgem.API.BUS.BusinessAccounts.Domain.Request;
using System;
using System.Collections.Generic;

namespace Ofgem.API.BUS.BusinessAccounts.Core.UnitTests.Automapper.Converters;

/// <summary>
/// Tests for the BusinessAccountUserConverter automapper 
/// </summary>
public class BusinessAccountUserConverterTests
{
    [Test]
    public void BusinessAccountUserConverter_Maps_PostUserAccountRequest_To_List_Of_ExternalUserAccount()
    {
        // Arrange
        var businessAccountId = Guid.NewGuid();
        var postUserAccountRequest = new PostUserAccountRequest
        {
            BusinessAccountID = businessAccountId,
            CreatedBy = "Unit Tests",
            ExternalUserAccounts = new List<PostUserAccountRequestExternalUserAccount>
            {
                new PostUserAccountRequestExternalUserAccount
                {
                    FirstName = "Test1",
                    LastName = "Testerson",
                    AuthorisedRepresentative = true,
                    EmailAddress = "test1@example.com",
                    StandardUser = false,
                    SuperUser = true
                },
                new PostUserAccountRequestExternalUserAccount
                {
                    FirstName = "Test2",
                    LastName = "Testerson",
                    AuthorisedRepresentative = false,
                    EmailAddress = "test2@example.com",
                    StandardUser = true,
                    SuperUser = false
                }
            }
        };

        var expected = new List<ExternalUserAccount> 
        { 
            new ExternalUserAccount
            {
                FirstName = "Test1",
                LastName = "Testerson",
                AuthorisedRepresentative = true,
                EmailAddress = "test1@example.com",
                StandardUser = false,
                SuperUser = true,
                BusinessAccountID=businessAccountId,
                CreatedBy = "Unit Tests"
            },
            new ExternalUserAccount
            {
                FirstName = "Test2",
                LastName = "Testerson",
                AuthorisedRepresentative = false,
                EmailAddress = "test2@example.com",
                StandardUser = true,
                SuperUser = false,
                BusinessAccountID = businessAccountId,
                CreatedBy = "Unit Tests"
            }
        };

        var mapper = new MapperConfiguration(mc =>
        {
            mc.CreateMap<PostUserAccountRequest, List<ExternalUserAccount>>()
            .ConvertUsing<BusinessAccountUserConverter>();
        }).CreateMapper();

        // Act
        var actual = mapper.Map<List<ExternalUserAccount>>(postUserAccountRequest);

        // Asssert
        actual.Should().BeEquivalentTo(expected);
    }
}
