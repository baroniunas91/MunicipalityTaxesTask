using MunicipalityTaxesAPI.Enums;

namespace MunicipalityTaxesAPI.Models.Responses
{
    public class TaxResponse
    {
        public int Id { get; set; }
        public string Municipality { get; set; }
        public TaxType Type { get; set; }
        public double TaxRate { get; set; }
        public TaxScheduleResponse TaxSchedule { get; set; }
    }
}