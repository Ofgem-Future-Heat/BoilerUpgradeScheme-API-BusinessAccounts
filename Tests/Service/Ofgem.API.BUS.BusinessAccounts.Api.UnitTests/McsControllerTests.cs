using NUnit.Framework;
using Ofgem.API.BUS.BusinessAccounts.Core.Interfaces;
using FluentAssertions;
using Ofgem.API.BUS.BusinessAccounts.Domain.Exceptions;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace Ofgem.API.BUS.BusinessAccounts.Api.UnitTests
{
    public class McsControllerTests
    {
        private McsController _mcsController;
        private readonly Mock<IMcsService> _mcsServiceMock = new();

        [SetUp]
        public void SetUp()
        {
            var mcsServiceMock = _mcsServiceMock.Object;
            _mcsController = new McsController(mcsServiceMock);
        }

        [Test]
        public void McsController_Ctor_Valid_NoException()
        {
            var result = () => new McsController(_mcsServiceMock.Object);

            result.Should().NotThrow();
        }

        [Test]
        public void McsController_Ctor_NoMcsService_Exception()
        {
            var result = () => new McsController(null!);

            result.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'mcsService')");
        }

        [Test]
        public async Task McsControllerShouldReturn204NoContentResponse()
        {
            //Arrange
            var mcsNumber = "123123";
            _mcsServiceMock.Setup(x => x.CheckMcsNumber(mcsNumber));

            var expectedResult = _mcsController.NoContent();

            //Act
            var result = await _mcsController.CheckMcsNumber(mcsNumber);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task McsControllerShouldReturn400BadRequestResponse()
        {
            //Arrange
            string? mcsNumber = null;
            _mcsServiceMock.Setup(x => x.CheckMcsNumber(mcsNumber!)).Throws(new BadRequestException("There was a error with your mcs number"));

            //Act
            var result = await _mcsController.CheckMcsNumber(mcsNumber!);

            //Assert
            result.Should().BeOfType(typeof(BadRequestObjectResult));
        }
    }
}