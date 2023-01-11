using FluentValidation;

namespace NZWALKS.Validators
{
    public class AddWalkRequestValidator : AbstractValidator<Models.DTO.AddWalkRequest>
    {
        public AddWalkRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Length).GreaterThanOrEqualTo(0);

        }
    }
}
