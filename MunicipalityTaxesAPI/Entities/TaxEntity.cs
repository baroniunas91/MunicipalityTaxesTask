using MunicipalityTaxesAPI.Enums;

namespace MunicipalityTaxesAPI.Entities
{
    public class TaxEntity
    {
        public int Id { get; set; }
        public TaxType Type { get; set; }
        public double TaxRate { get; set; }
        public int TaxScheduleId { get; set; }
        public int MunicipalityId { get; set; }
    }
}
