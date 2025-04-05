using AutoMapper;
using MunicipalityTaxesAPI.Entities;
using MunicipalityTaxesAPI.Enums;
using MunicipalityTaxesAPI.Exceptions;
using MunicipalityTaxesAPI.Interfaces;
using MunicipalityTaxesAPI.Models.Requests;
using MunicipalityTaxesAPI.Models.Responses;
using System.Net;

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

        public async Task<IEnumerable<TaxResponse>> GetAllTaxesAsync(TaxGetRequest taxGetRequest, CancellationToken ct)
        {
            var taxes = await _taxRepository.GetAllAsync(taxGetRequest, ct);

            return _mapper.Map<IEnumerable<TaxEntity>, IEnumerable<TaxResponse>>(taxes);
        }

        public async Task<TaxResponse> CreateTaxAsync(TaxCreateRequest taxCreateRequest, CancellationToken ct)
        {
            var newTax = _mapper.Map<TaxEntity>(taxCreateRequest);
            
            CalculateTaxSchedulePeriodEndDate(newTax);

            await _taxRepository.AddAsync(newTax, ct);
            await _taxRepository.SaveChangesAsync(ct);

            return _mapper.Map<TaxResponse>(newTax);
        }

        public async Task<TaxByMunicipalityandDateResponse> GetByMunicipalityAndDateAsync(TaxGetRequest taxGetRequest, CancellationToken ct)
        {
            var taxes = await _taxRepository.GetAllAsync(taxGetRequest, ct);

            var highestPriorityTax = taxes.OrderBy(x => x.Type).FirstOrDefault();

            if (highestPriorityTax != null)
            {
                return _mapper.Map<TaxByMunicipalityandDateResponse>(highestPriorityTax);
            }

            throw new HttpStatusException(HttpStatusCode.NotFound, "Tax not found");
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
