using Microsoft.AspNetCore.Mvc;
using MunicipalityTaxesAPI.Interfaces;
using MunicipalityTaxesAPI.Models.Requests;
using System.ComponentModel.DataAnnotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MunicipalityTaxesAPI.Controllers
{
    [Route("taxes")]
    [ApiController]
    public class TaxController : ControllerBase
    {
        private readonly ITaxService _taxService;

        public TaxController(ITaxService taxService)
        {
            _taxService = taxService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] TaxGetRequest request, CancellationToken ct)
        {
            var result = await _taxService.GetTaxes(request, ct);
            return Ok(result);
        }

        //// GET api/<TaxController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        [HttpPost]
        public async Task<IActionResult> CreateTax([FromBody] TaxCreateRequest taxCreateRequest, CancellationToken ct)
        {
            //var validationResult = await validator.ValidateAsync(taskCreateRequest);
            //if (!validationResult.IsValid)
            //{
            //    return BadRequest(new ValidationErrorResult(validationResult.Errors));
            //}

            var result = await _taxService.CreateTaxAsync(taxCreateRequest, ct);
            return Created("~/taxes/" + result.Id, result);
        }

        //// PUT api/<TaxController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<TaxController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
