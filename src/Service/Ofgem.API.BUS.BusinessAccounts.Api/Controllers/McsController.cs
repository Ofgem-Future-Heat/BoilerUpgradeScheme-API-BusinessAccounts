using Microsoft.AspNetCore.Mvc;
using Ofgem.API.BUS.BusinessAccounts.Api.Extensions;
using Ofgem.API.BUS.BusinessAccounts.Core;
using Ofgem.API.BUS.BusinessAccounts.Core.Interfaces;
using Ofgem.API.BUS.BusinessAccounts.Domain.Exceptions;
using Ofgem.API.BUS.BusinessAccounts.Domain.Request;

namespace Ofgem.API.BUS.BusinessAccounts.Api
{
    /// <summary>
    /// This Mcs controller routes all Mcs oprations to the Mcs Service class.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class McsController : ControllerBase
    {
        private readonly IMcsService _mcsService;


        public McsController(IMcsService mcsService)
        {
            _mcsService = mcsService ?? throw new ArgumentNullException(nameof(mcsService));
        }
        /// <summary>
        /// This Controller function routes the MCS Number to be checked and validated in CheckMcsNumber(),
        /// in the Mcs Service class.
        /// </summary>
        /// <param name="mcsNumber"></param>
        /// <returns></returns>
        [Route("CheckMcsNumber")]
        [HttpGet]
        public async Task<IActionResult> CheckMcsNumber(string mcsNumber)
        {
            try
            {
                await _mcsService.CheckMcsNumber(mcsNumber);
            }
            catch (BadRequestException ex)
            {
                return this.AsObjectResult(ex);
            }
            return NoContent();
        }
    }
}