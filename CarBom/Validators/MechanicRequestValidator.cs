using CarBom.Requests;
using FluentValidation;

namespace CarBom.Validators
{
    public class MechanicRequestValidator : AbstractValidator<MechanicRequest>
    {
        public MechanicRequestValidator()
        {
            RuleFor(x => x).NotNull();

            RuleFor(x => x.Name).NotEmpty().WithMessage("'Name' cannot be null");

            RuleFor(x => x.Address).NotNull().WithMessage("'Address' is required to create a Mechanic");

            RuleFor(x => x.Address.State).NotNull().WithMessage("'Address.State' is required to create a Mechanic");

            RuleFor(x => x.Address.City).NotNull().WithMessage("'Address.City' is required to create a Mechanic");

            RuleFor(x => x.Address.Street).NotNull().WithMessage("'Address.Street' is required to create a Mechanic");

            RuleFor(x => x.Address.Number).NotNull().WithMessage("'Address.Number' is required to create a Mechanic");

            RuleFor(x => x.Address.ZipPostalCode).NotNull().WithMessage("'Address.ZipPostalCode' is required to create a Mechanic");

            RuleFor(x => x.Address.Longitude).NotNull().WithMessage("'Address.Longitude' is required to create a Mechanic")
                .NotEqual(0).WithMessage("'Address.Longitude' must be a valid Longitude");

            RuleFor(x => x.Address.Latitude).NotNull().WithMessage("'Address.Latitude' is required to create a Mechanic")
                .NotEqual(0).WithMessage("'Address.Latitude' must be a valid Latitude");
        }
    }
}
