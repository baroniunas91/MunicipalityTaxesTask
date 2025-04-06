using FluentValidation;
using MunicipalityTaxesAPI.Helpers;
using MunicipalityTaxesAPI.Interfaces;
using MunicipalityTaxesAPI.Models.Requests;

namespace MunicipalityTaxesAPI.Validators
{
    public class TaxUpdateRequestValidator : AbstractValidator<TaxUpdateRequest>, IFluentValidator
    {
        public TaxUpdateRequestValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            When(x => x.Municipality != null, () =>
            {
                RuleFor(x => x.Municipality).NotEmpty();
            });

            When(x => x.TaxRate != null, () =>
            {
                RuleFor(x => x.TaxRate).GreaterThan(0);
                RuleFor(x => x.TaxRate).LessThan(1);
            });

            When(x => x.TaxSchedule?.PeriodStart != null, () =>
            {
                RuleFor(x => x.TaxSchedule.PeriodStart)
                .Must(ValidatorHelper.ValidDate)
                .WithMessage("Invalid date"); ;
            });
        }
    }
}
