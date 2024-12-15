using FluentValidation;
using ICT.Strypes.Business.Models;
using ICT.Strypes.Business.Resources;
using ICT.Strypes.Domain.Entities;

namespace ICT.Strypes.Business.Validators
{
    public class PatchLocationRequestModelValidator : AbstractValidator<PatchLocationRequestModel>
    {
        public PatchLocationRequestModelValidator()
        {
            RuleFor(location => location.Type)
               .MaximumLength(45).WithMessage(string.Format(ErrorMessages.FieldMaximumLengthErrorMessage, 45))
               .Must(BeAValidLocationType).WithMessage(ErrorMessages.LocationTypeNotValidErrorMessage)
               .When(location => !string.IsNullOrEmpty(location.Type));

            RuleFor(location => location.Name)
               .MaximumLength(255).WithMessage(string.Format(ErrorMessages.FieldMaximumLengthErrorMessage, 255));

            RuleFor(location => location.Address)
               .MaximumLength(45).WithMessage(string.Format(ErrorMessages.FieldMaximumLengthErrorMessage, 45));

            RuleFor(location => location.City)
               .MaximumLength(45).WithMessage(string.Format(ErrorMessages.FieldMaximumLengthErrorMessage, 45));

            RuleFor(location => location.PostalCode)
               .MaximumLength(10).WithMessage(string.Format(ErrorMessages.FieldMaximumLengthErrorMessage, 10));

            RuleFor(location => location.Country)
               .MaximumLength(45).WithMessage(string.Format(ErrorMessages.FieldMaximumLengthErrorMessage, 45));
        }

        private bool BeAValidLocationType(string? type)
        {
            return Enum.IsDefined(typeof(LocationType), type!);
        }
    }
}
