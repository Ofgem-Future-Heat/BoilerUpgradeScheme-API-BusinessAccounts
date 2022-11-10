using FluentValidation;
using Ofgem.API.BUS.BusinessAccounts.Domain.Entities;

namespace Ofgem.API.BUS.BusinessAccounts.Core.FluentValidation;

public class ExternalUserAccountRequestValidator : AbstractValidator<ExternalUserAccount>
{
    public ExternalUserAccountRequestValidator()
    {
        RuleFor(x => x.EmailAddress).EmailAddress().NotEmpty();
    }
}
