using System.Runtime.Serialization;

namespace MunicipalityTaxesAPI.Enums
{
    public enum TaxType
    {
        [EnumMember(Value = "Daily")]
        Daily = 0,

        [EnumMember(Value = "Weekly")]
        Weekly = 1,

        [EnumMember(Value = "Monthly")]
        Monthly = 3,

        [EnumMember(Value = "Yearly")]
        Yearly = 4
    }
}