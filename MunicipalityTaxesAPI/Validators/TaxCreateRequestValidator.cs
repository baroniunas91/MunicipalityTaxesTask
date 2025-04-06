using FluentValidation;
using MunicipalityTaxesAPI.Helpers;
using MunicipalityTaxesAPI.Interfaces;
using MunicipalityTaxesAPI.Models.Requests;

namespace MunicipalityTaxesAPI.Validators
{
    public class TaxCreateRequestValidator : AbstractValidator<TaxCreateRequest>, IFluentValidator
    {
        public TaxCreateRequestValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Municipality).NotEmpty();
            
            RuleFor(x => x.TaxRate).GreaterThan(0);
            
            RuleFor(x => x.TaxRate).LessThan(1);
            
            RuleFor(x => x.TaxSchedule).NotNull();
            
            RuleFor(x => x.TaxSchedule.PeriodStart).NotNull();
            
            RuleFor(x => x.TaxSchedule.PeriodStart)
                .Must(x => ValidatorHelper.ValidDate(x))
                .WithMessage("Invalid date");
        }
    }
}