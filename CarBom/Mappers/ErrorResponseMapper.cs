using CarBom.Responses;
using FluentValidation.Results;

namespace CarBom.Mappers
{
    public class ErrorResponseMapper : IErrorResponseMapper
    {
        public List<ResultDetail> Map(ValidationResult validationResult)
        {
            if (validationResult == null || validationResult.Errors == null) return new List<ResultDetail>(0);

            List<ResultDetail> resultDetails = new List<ResultDetail>();

            foreach (var error in validationResult.Errors)
            {
                resultDetails.Add(new ResultDetail { Message = error.ErrorMessage });
            }
            return resultDetails;
        }
    }

    public interface IErrorResponseMapper
    {
        List<ResultDetail> Map(ValidationResult validationResult);
    }
}
