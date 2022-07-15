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
           

            RuleFor(x => x.Password).NotEmpty().WithMessage("Password không được trống")
                .MinimumLength(6).WithMessage("Password ít nhất 6 kí tự ");

            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Password không được trống")
                .MinimumLength(6).WithMessage("Password ít nhất 6 kí tự ");

            RuleFor(x => x).Custom((request, context) =>
            {
                if (request.Password != request.ConfirmPassword)
                {
                    context.AddFailure("Confirm password không khớp");
                }
            });
        }
    }
}
