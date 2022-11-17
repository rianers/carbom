using CarBom.Requests;
using FluentValidation;

namespace CarBom.Validators
{
    public class OrderedServiceRequestValidator : AbstractValidator<OrderedServiceRequest>
    {
        public OrderedServiceRequestValidator()
        {
            RuleFor(x => x).NotNull();
            RuleFor(x => x.ServiceId).NotEmpty().WithMessage("'ServiceId' cannot be null");
            RuleFor(x => x.UserId).NotEmpty().WithMessage("'UserId' cannot be null");
            RuleFor(x => x.MechanicId).NotEmpty().WithMessage("'MechanicId' cannot be null");
        }
    }
}
