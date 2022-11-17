using CarBom.Requests;
using FluentValidation;

namespace CarBom.Validators
{
    public class UserRequestValidator : AbstractValidator<UserRequest>
    {
        public UserRequestValidator()
        {
            RuleFor(x => x).NotNull();
            RuleFor(x => x.Email).NotEmpty().WithMessage("'Email' cannot be null");
            RuleFor(x => x.Password).NotEmpty().WithMessage("'Password' cannot be null");
        }
    }
}
