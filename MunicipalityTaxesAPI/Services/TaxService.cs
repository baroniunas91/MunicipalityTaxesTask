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

            newTax.TaxSchedule.PeriodEnd = GetTaxSchedulePeriodEndDate(newTax.Type, newTax.TaxSchedule.PeriodStart);

            await _taxRepository.AddAsync(newTax, ct);
            await _taxRepository.SaveChangesAsync(ct);

            return _mapper.Map<TaxResponse>(newTax);
        }

        public async Task<TaxByMunicipalityandDateResponse> GetByMunicipalityAndDateAsync(TaxGetRequest taxGetRequest, CancellationToken ct)
        {
            if (taxGetRequest.Municipality == null || taxGetRequest.Date == null)
            {
                throw new HttpStatusException(HttpStatusCode.BadRequest, "Municipality and date query params are required");
            }

            var taxes = await _taxRepository.GetAllAsync(taxGetRequest, ct);

            // Assume that highest tax priority is daily tax. Priority order described in TaxType Enum.
            var highestPriorityTax = taxes.OrderBy(x => x.Type).FirstOrDefault();

            if (highestPriorityTax != null)
            {
                return _mapper.Map<TaxByMunicipalityandDateResponse>(highestPriorityTax);
            }

            throw new HttpStatusException(HttpStatusCode.NotFound, "Tax not found");
        }

        public async Task<TaxResponse> UpdateTaxAsync(int taxId, TaxUpdateRequest taxUpdateRequest, CancellationToken ct)
        {
            var tax = await _taxRepository.FindSingleAsync(x => x.Id == taxId, ct);
            
            if (tax == null)
            {
                throw new HttpStatusException(HttpStatusCode.NotFound, "Tax not found");
            }

            _mapper.Map(taxUpdateRequest, tax);


            if (taxUpdateRequest.Type != null || taxUpdateRequest.TaxSchedule?.PeriodStart != null)
            {
                tax.TaxSchedule.PeriodEnd = GetTaxSchedulePeriodEndDate(tax.Type, tax.TaxSchedule.PeriodStart);
            }

            _taxRepository.Update(tax);
            
            await _taxRepository.SaveChangesAsync(ct);

            return _mapper.Map<TaxResponse>(tax);
        }


        /// <summary>
        /// Get tax schedule period end date. Note: period end date calculated up to next day. 
        /// For example: Period Start 2024-01-01 00:00:00AM Period End 2024-01-02 00:00:00AM. It means that 02 day tax is not applied, tax applied only 01 day.
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="periodStart">periodStart</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public DateTime GetTaxSchedulePeriodEndDate(TaxType type, DateTime periodStart)
        {
            var periodEnd = type switch
            {
                TaxType.Daily => periodStart.AddDays(1),
                TaxType.Weekly => periodStart.AddDays(7),
                TaxType.Monthly => periodStart.AddMonths(1),
                TaxType.Yearly => periodStart.AddYears(1),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
            };

            return periodEnd;
        }
    }
}