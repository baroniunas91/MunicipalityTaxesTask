using MunicipalityTaxesAPI.Enums;

namespace MunicipalityTaxesAPI.Models.Requests
{
    public class TaxCreateRequest
    {
        public string Municipality { get; set; }
        public TaxType Type { get; set; }
        public double TaxRate { get; set; }
        public TaxScheduleCreateRequest TaxSchedule { get; set; }
    }
}
