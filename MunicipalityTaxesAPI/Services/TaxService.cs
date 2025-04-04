using AutoMapper;
using MunicipalityTaxesAPI.Entities;
using MunicipalityTaxesAPI.Enums;
using MunicipalityTaxesAPI.Interfaces;
using MunicipalityTaxesAPI.Models.Requests;
using MunicipalityTaxesAPI.Models.Responses;

namespace MunicipalityTaxesAPI.Services
{
    public class TaxService : ITaxService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<TaxEntity> _taxRepository;

        public TaxService(IMapper mapper, IRepository<TaxEntity> taxRepository)
        {
            _mapper = mapper;
            _taxRepository = taxRepository;
        }

        public async Task<TaxResponse> GetTaxes(TaxGetRequest taxGetRequest, CancellationToken ct)
        {
            return new TaxResponse();
        }

        public async Task<TaxResponse> CreateTaxAsync(TaxCreateRequest taxCreateRequest, CancellationToken ct)
        {
            var newTax = _mapper.Map<TaxEntity>(taxCreateRequest);
            
            CalculateTaxSchedulePeriodEndDate(newTax);

            await _taxRepository.AddAsync(newTax);
            await _taxRepository.SaveChangesAsync();

            return _mapper.Map<TaxResponse>(newTax);
        }

        private static void CalculateTaxSchedulePeriodEndDate(TaxEntity newTax)
        {
            newTax.TaxSchedule.PeriodEnd = newTax.Type switch
            {
                TaxType.Daily => newTax.TaxSchedule.PeriodStart.AddDays(1),
                TaxType.Weekly => newTax.TaxSchedule.PeriodStart.AddDays(7),
                TaxType.Monthly => newTax.TaxSchedule.PeriodStart.AddMonths(1),
                TaxType.Yearly => newTax.TaxSchedule.PeriodStart.AddYears(1),
                _ => throw new ArgumentOutOfRangeException(nameof(newTax.Type), newTax.Type, null),
            };
        }
    }
}
