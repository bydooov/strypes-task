using FluentValidation;
using ICT.Strypes.Business.Models;
using ICT.Strypes.Business.Resources;
using ICT.Strypes.Domain.Entities;

namespace ICT.Strypes.Business.Validators
{
    public class LocationRequestModelValidator : AbstractValidator<LocationRequestModel>
    {
        public LocationRequestModelValidator()
        {
            RuleFor(location => location.LocationId)
                .NotEmpty().WithMessage(string.Format(ErrorMessages.FieldRequiredErrorMessage, "LocationId"))
                .MaximumLength(39).WithMessage(string.Format(ErrorMessages.FieldMaximumLengthErrorMessage, 39));

            RuleFor(location => location.Type)
               .NotEmpty().WithMessage(string.Format(ErrorMessages.FieldRequiredErrorMessage, "Type"))
               .MaximumLength(45).WithMessage(string.Format(ErrorMessages.FieldMaximumLengthErrorMessage, 45))
               .Must(BeAValidLocationType).WithMessage(ErrorMessages.LocationTypeNotValidErrorMessage);

            RuleFor(location => location.Name)
               .MaximumLength(255).WithMessage(string.Format(ErrorMessages.FieldMaximumLengthErrorMessage, 255));

            RuleFor(location => location.Address)
               .NotEmpty().WithMessage(string.Format(ErrorMessages.FieldRequiredErrorMessage, "Address"))
               .MaximumLength(45).WithMessage(string.Format(ErrorMessages.FieldMaximumLengthErrorMessage, 45));

            RuleFor(location => location.City)
               .NotEmpty().WithMessage(string.Format(ErrorMessages.FieldRequiredErrorMessage, "City"))
               .MaximumLength(45).WithMessage(string.Format(ErrorMessages.FieldMaximumLengthErrorMessage, 45));

            RuleFor(location => location.PostalCode)
               .NotEmpty().WithMessage(string.Format(ErrorMessages.FieldRequiredErrorMessage, "PostalCode"))
               .MaximumLength(10).WithMessage(string.Format(ErrorMessages.FieldMaximumLengthErrorMessage, 10));

            RuleFor(location => location.Country)
               .NotEmpty().WithMessage(string.Format(ErrorMessages.FieldRequiredErrorMessage, "Country"))
               .MaximumLength(45).WithMessage(string.Format(ErrorMessages.FieldMaximumLengthErrorMessage, 45));

            RuleFor(location => location.LastUpdated)
               .NotEmpty().WithMessage(string.Format(ErrorMessages.FieldRequiredErrorMessage, "LastUpdated"));
        }

        private bool BeAValidLocationType(string? type)
        {
            return Enum.IsDefined(typeof(LocationType), type!);
        }
    }
}
