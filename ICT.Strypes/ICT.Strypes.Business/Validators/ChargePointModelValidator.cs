using FluentValidation;
using ICT.Strypes.Business.Models;
using ICT.Strypes.Business.Resources;
using ICT.Strypes.Domain.Entities;

namespace ICT.Strypes.Business.Validators
{
    public class ChargePointModelValidator : AbstractValidator<ChargePointModel>
    {
        public ChargePointModelValidator()
        {
            RuleFor(cp => cp.ChargePointId)
                .NotEmpty().WithMessage(string.Format(ErrorMessages.FieldRequiredErrorMessage, "ChargePointId"))
                .MaximumLength(39).WithMessage(string.Format(ErrorMessages.FieldMaximumLengthErrorMessage, 39));

            RuleFor(cp => cp.Status)
               .NotEmpty().WithMessage(string.Format(ErrorMessages.FieldRequiredErrorMessage, "Status"))
               .MaximumLength(39).WithMessage(string.Format(ErrorMessages.FieldMaximumLengthErrorMessage, 39))
               .Must(BeAValidChargePointStatus).WithMessage(ErrorMessages.ChargePointStatusNotValidErrorMessage)
               .When(cp => !string.IsNullOrEmpty(cp.Status));

            RuleFor(cp => cp.FloorLevel)
               .MaximumLength(4).WithMessage(string.Format(ErrorMessages.FieldMaximumLengthErrorMessage, 4));

            RuleFor(cp => cp.LastUpdated)
               .NotEmpty().WithMessage(string.Format(ErrorMessages.FieldRequiredErrorMessage, "LastUpdated"));
        }

        private bool BeAValidChargePointStatus(string? status)
        {
            return Enum.IsDefined(typeof(ChargePointStatus), status!);
        }
    }
}
