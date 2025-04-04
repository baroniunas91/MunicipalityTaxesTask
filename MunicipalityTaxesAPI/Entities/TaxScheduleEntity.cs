using MunicipalityTaxesAPI.Interfaces;

namespace MunicipalityTaxesAPI.Entities
{
    public class TaxScheduleEntity : IEntity
    {
        public int Id { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public TaxEntity Tax { get; set; }
        public int TaxId { get; set; }

    }
}
