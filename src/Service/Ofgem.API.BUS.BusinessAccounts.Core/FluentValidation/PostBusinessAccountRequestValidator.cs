using FluentValidation;
using Ofgem.API.BUS.BusinessAccounts.Domain.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ofgem.API.BUS.BusinessAccounts.Core.FluentValidation
{
    public class PostBusinessAccountRequestValidator : AbstractValidator<PostBusinessAccountRequest>
    {
        /// <summary>
        /// this method applies validation to the PostBusinessAccountRequest type to ensure data is provided valid
        /// </summary>
        public PostBusinessAccountRequestValidator()
        {
            RuleFor(x => x.BusinessName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.MCSCertificationNumber).NotEmpty();
            RuleFor(x => x.BusinessAddress).NotEmpty();
            RuleFor(x => x.DateAccountReceived).NotEmpty();
        }
    }
}
