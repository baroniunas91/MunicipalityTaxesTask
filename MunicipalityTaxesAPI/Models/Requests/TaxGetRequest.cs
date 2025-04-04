using Microsoft.AspNetCore.Mvc;

namespace MunicipalityTaxesAPI.Models.Requests
{
    public class TaxGetRequest
    {
        [FromQuery(Name = "name")] public string Name { get; set; }

        [FromQuery(Name = "date")] public string Date { get; set; }
    }
}
