using Microsoft.AspNetCore.Mvc;
using MunicipalityTaxesAPI.Interfaces;
using MunicipalityTaxesAPI.Models;
using MunicipalityTaxesAPI.Models.Requests;
using MunicipalityTaxesAPI.Models.Responses;
using MunicipalityTaxesAPI.Validators;

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
        public async Task<IActionResult> GetAll([FromQuery] TaxGetRequest request, CancellationToken ct)
        {
            var result = await _taxService.GetAllTaxesAsync(request, ct);
            
            return Ok(result);
        }

        [HttpGet("single")]
        public async Task<IActionResult> GetSingleByQueryParams([FromQuery] TaxGetRequest request, CancellationToken ct)
        {
            var result = await _taxService.GetByMunicipalityAndDateAsync(request, ct);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTax([FromBody] TaxCreateRequest taxCreateRequest, [FromServices] TaxCreateRequestValidator validator, CancellationToken ct)
        {
            var validationResult = await validator.ValidateAsync(taxCreateRequest, ct);
            if (!validationResult.IsValid)
            {
                return BadRequest(new ValidationErrorResponse(validationResult.Errors));
            }

            var result = await _taxService.CreateTaxAsync(taxCreateRequest, ct);
            return Created("~/taxes/" + result.Id, result);
        }

        [HttpPut("{taxId}")]
        [HttpPatch("{taxId}")]
        public async Task<IActionResult> UpdateTax(int taxId, [FromBody] TaxUpdateRequest taxUpdateRequest, [FromServices] TaxUpdateRequestValidator validator, CancellationToken ct)
        {
            var validationResult = await validator.ValidateAsync(taxUpdateRequest, ct);
            if (!validationResult.IsValid)
            {
                return BadRequest(new ValidationErrorResponse(validationResult.Errors));
            }

            await _taxService.UpdateTaxAsync(taxId, taxUpdateRequest, ct);
            
            return Ok(new ResponseMessage("Tax was successfully updated"));
        }
    }
}