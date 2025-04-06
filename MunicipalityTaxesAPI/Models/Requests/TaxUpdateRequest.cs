using MunicipalityTaxesAPI.Enums;

namespace MunicipalityTaxesAPI.Models.Requests
{
    public class TaxUpdateRequest
    {
        public string Municipality { get; set; }
        public TaxType? Type { get; set; }
        public double? TaxRate { get; set; }
        public TaxScheduleUpdateRequest TaxSchedule { get; set; }
    }
}
