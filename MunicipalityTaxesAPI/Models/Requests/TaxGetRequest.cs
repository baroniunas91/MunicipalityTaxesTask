using Microsoft.AspNetCore.Mvc;
using MunicipalityTaxesAPI.Interfaces;

namespace MunicipalityTaxesAPI.Models.Requests
{
    public class TaxGetRequest : IBaseGetRequest
    {
        [FromQuery(Name = "municipality")] public string Municipality { get; set; }

        [FromQuery(Name = "date")] public DateTime? Date { get; set; }
    }
}