using CarBom.Mappers;
using CarBom.Requests;
using CarBom.Responses;
using DataProvider.DataModels;
using DataProvider.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CarBom.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IErrorResponseMapper _errorResponseMapper;
        private readonly IValidator<ServiceRequest> _serviceRequestValidator;

        public ServiceController(IServiceRepository serviceRepository,
                                 IErrorResponseMapper errorResponseMapper,
                                 IValidator<ServiceRequest> serviceRequestValidator)
        {
            _serviceRepository = serviceRepository;
            _errorResponseMapper = errorResponseMapper;
            _serviceRequestValidator = serviceRequestValidator;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ServiceResponse>> Post([FromBody] ServiceRequest serviceRequest, string mechanicId)
        {
            var validationResponse = GetServiceResponse(serviceRequest);
            if (validationResponse.ResultCode is ResultConstants.SUCCESS)
            {
                Service service = new Service
                {
                    Name = serviceRequest.Name,
                    Image = serviceRequest.Image,
                    Price = serviceRequest.Price
                };
                await _serviceRepository.Post(service, mechanicId);
                return Ok(validationResponse);
            }
            return BadRequest(validationResponse);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<Service>> Get([FromQuery] string mechanicId)
        {
            var services = _serviceRepository.Get(mechanicId);

            if (services is not null)
                return Ok(services);
            else
                return NotFound();
        }

        private ServiceResponse GetServiceResponse(ServiceRequest serviceRequest)
        {
            ServiceResponse? serviceResponse = null;
            var validationResponse = _serviceRequestValidator.Validate(serviceRequest);
            if (validationResponse is not null && !validationResponse.IsValid)
            {
                List<ResultDetail> resultDetails = _errorResponseMapper.Map(validationResponse);
                serviceResponse = new ServiceResponse
                {
                    ResultCode = ResultConstants.ERROR,
                    ResultDetails = resultDetails
                };
            }
            else
            {
                serviceResponse = new ServiceResponse
                {
                    ResultCode = ResultConstants.SUCCESS
                };
            }
            return serviceResponse;
        }
    }
}
