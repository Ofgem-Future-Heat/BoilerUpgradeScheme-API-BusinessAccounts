using AutoMapper;
using FluentAssertions;
using Moq;
using Notify.Interfaces;
using Notify.Models.Responses;
using NUnit.Framework;
using Ofgem.API.BUS.BusinessAccounts.Core.Interfaces;
using Ofgem.API.BUS.BusinessAccounts.Domain.CommsObjects;
using Ofgem.API.BUS.BusinessAccounts.Domain.Entities;
using Ofgem.API.BUS.BusinessAccounts.Domain.Exceptions;
using Ofgem.API.BUS.BusinessAccounts.Domain.Request;
using Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ofgem.API.BUS.BusinessAccounts.Core.UnitTests.InviteServiceTests
{
    public class InviteTokenServiceOptionsTests
    {
        protected InviteTokenServiceOptions inviteTokenServiceOptions;

        [Test]
        public void InviteService_Ctor_Valid_NoException()
        {
            var result = () => new InviteTokenServiceOptions("tokenEmailTemplateId", "inviteTokenSecret", "externalPortalUrl", "installerReplyToAddress");

            result.Should().NotThrow();
        }

        [Test]
        public void InviteService_Ctor_NoEmailTemplate_Exception()
        {
            var result = () => new InviteTokenServiceOptions(null!, "inviteTokenSecret", "externalPortalUrl", "installerReplyToAddress");

            result.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'tokenEmailTemplateId')");
        }

        [Test]
        public void InviteService_Ctor_NoInviteTokenSecret_Exception()
        {
            var result = () => new InviteTokenServiceOptions("tokenEmailTemplateId", null!, "externalPortalUrl", "installerReplyToAddress");

            result.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'inviteTokenSecret')");
        }

        [Test]
        public void InviteService_Ctor_NoUrl_Exception()
        {
            var result = () => new InviteTokenServiceOptions("tokenEmailTemplateId", "inviteTokenSecret", null!, "installerReplyToAddress");

            result.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'externalPortalUrl')");
        }

        [Test]
        public void InviteService_Ctor_NoReplyTo_Exception()
        {
            var result = () => new InviteTokenServiceOptions("tokenEmailTemplateId", "inviteTokenSecret", "externalPortalUrl", null!);

            result.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'installerReplyToAddress')");
        }
    }
}
