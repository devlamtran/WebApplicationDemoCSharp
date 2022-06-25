using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplicationLogic.Catalog.Users.Dto
{
    public class ResetPassRequestValidator : AbstractValidator<ResetPasswordViewModel>
    {
        public ResetPassRequestValidator()
        {
            RuleFor(x => x.OldPassword).NotEmpty().WithMessage("Password is required")
               .MinimumLength(6).WithMessage("Password is at least 6 characters");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password is at least 6 characters");

            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password is at least 6 characters");

            RuleFor(x => x).Custom((request, context) =>
            {
                if (request.Password != request.ConfirmPassword)
                {
                    context.AddFailure("Confirm password is not match");
                }
            });
        }
    }
}
