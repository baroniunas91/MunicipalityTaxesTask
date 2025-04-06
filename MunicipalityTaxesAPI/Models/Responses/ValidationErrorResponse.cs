using FluentValidation.Results;

namespace MunicipalityTaxesAPI.Models.Responses
{
    public class ValidationErrorResponse
    {
        public string Message { get; set; }
        public ErrorModel[] Errors { get; set; }
        public ValidationErrorResponse(IEnumerable<ValidationFailure> errors)
        {
            var result =
                errors
                    .Select(x => new ErrorModel
                    {
                        Key = x.PropertyName,
                        Errors = [x.ErrorMessage]
                    })
                    .ToArray();

            Message = "The given data was invalid.";
            Errors = result;
        }
    }
}