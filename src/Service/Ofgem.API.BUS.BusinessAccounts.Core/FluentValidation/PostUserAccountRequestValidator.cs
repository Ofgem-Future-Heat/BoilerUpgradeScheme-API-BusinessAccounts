using FluentValidation;
using Ofgem.API.BUS.BusinessAccounts.Domain.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ofgem.API.BUS.BusinessAccounts.Core.FluentValidation
{
    public class PostUserAccountRequestValidator : AbstractValidator<PostUserAccountRequest>
    {
        /// <summary>
        /// This method applies fluent validation to the PostUserAccountRequest type
        /// </summary>
        public PostUserAccountRequestValidator()
        {
            RuleFor(x => x.BusinessAccountID).NotEmpty();
            RuleFor(x => x.ExternalUserAccounts).NotEmpty();
        }
    }
}
