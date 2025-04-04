using MunicipalityTaxesAPI.Models.Requests;
using MunicipalityTaxesAPI.Models.Responses;

namespace MunicipalityTaxesAPI.Interfaces
{
    public interface ITaxService
    {
        Task<TaxResponse> GetTaxes(TaxGetRequest taxGetRequest, CancellationToken ct);
        Task<TaxResponse> CreateTaxAsync(TaxCreateRequest taxCreateRequest, CancellationToken ct);
    }
}
