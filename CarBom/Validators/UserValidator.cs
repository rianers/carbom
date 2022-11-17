using DataProvider.DataModels;
using FluentValidation;

namespace CarBom.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x).NotNull();
            RuleFor(x => x.Email).NotEmpty().WithMessage("'Email' cannot be null");
            RuleFor(x => x.Name).NotEmpty().WithMessage("'Name' cannot be null");
            RuleFor(x => x.Password).NotEmpty().WithMessage("'Password' cannot be null");
        }
    }
}
