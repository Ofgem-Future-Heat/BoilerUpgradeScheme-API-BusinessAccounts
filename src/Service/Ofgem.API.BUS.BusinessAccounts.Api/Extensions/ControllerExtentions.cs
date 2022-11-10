using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ofgem.API.BUS.BusinessAccounts.Domain.Exceptions;
using System.Linq;
using System.Net;

namespace Ofgem.API.BUS.BusinessAccounts.Api.Extensions
{
    /// <summary>
    /// Controller extensions
    /// </summary>
    public static class ControllerExtensions
    {
        /// <summary>
        /// Bad request object result.
        /// </summary>
        /// <param name="controllerBase">The controller.</param>
        /// <param name="ex">BadRequestException.</param>
        /// <returns></returns>
        public static ActionResult AsObjectResult(this ControllerBase controllerBase, BadRequestException ex)
        {
            var request = FormatRequest(ex);
            if (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return controllerBase.NotFound(request);
            }
            else if (ex.StatusCode == HttpStatusCode.NoContent)
            {
                return controllerBase.NoContent();
            }
            return controllerBase.BadRequest(request);
        }

        private static object FormatRequest(BadRequestException ex)
        {
            if (ex.Errors != null && ex.Errors.Any())
            {
                return new { title = ex.Message, status = ex.StatusCode, errors = ex.Errors };
            }
            return new { title = ex.Message, status = ex.StatusCode };
        }
    }
}