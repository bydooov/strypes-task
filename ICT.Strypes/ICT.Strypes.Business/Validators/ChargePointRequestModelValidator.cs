using FluentValidation;
using ICT.Strypes.Business.Models;

namespace ICT.Strypes.Business.Validators
{
    public class ChargePointRequestModelValidator : AbstractValidator<ChargePointRequestModel>
    {
        public ChargePointRequestModelValidator()
        {
            RuleFor(cp => cp.ChargePoints)
            .ForEach(cp => cp.SetValidator(new ChargePointModelValidator()));
        }
    }
}
