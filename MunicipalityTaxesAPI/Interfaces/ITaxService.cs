using MunicipalityTaxesAPI.Models.Requests;
using MunicipalityTaxesAPI.Models.Responses;

namespace MunicipalityTaxesAPI.Interfaces
{
    public interface ITaxService
    {
        Task<IEnumerable<TaxResponse>> GetAllTaxesAsync(TaxGetRequest taxGetRequest, CancellationToken ct);
        Task<TaxResponse> CreateTaxAsync(TaxCreateRequest taxCreateRequest, CancellationToken ct);
        Task<TaxByMunicipalityandDateResponse> GetByMunicipalityAndDateAsync(TaxGetRequest taxGetRequest, CancellationToken ct);
        Task<TaxResponse> UpdateTaxAsync(int taxId, TaxUpdateRequest taxUpdateRequest, CancellationToken ct);
    }
}
