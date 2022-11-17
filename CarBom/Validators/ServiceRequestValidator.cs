using CarBom.Requests;
using FluentValidation;

namespace CarBom.Validators
{
    public class ServiceRequestValidator : AbstractValidator<ServiceRequest>
    {
        public ServiceRequestValidator()
        {
            RuleFor(x => x).NotNull();
            RuleFor(x => x.Name).NotEmpty().WithMessage("'Name' cannot be null");
        }
    }
}
