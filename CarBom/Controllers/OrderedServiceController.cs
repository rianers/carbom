using CarBom.Mappers;
using CarBom.Requests;
using CarBom.Responses;
using DataProvider.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CarBom.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderedServiceController : ControllerBase
    {
        private readonly IOrderedServiceRepository _orderedServiceRepository;
        private readonly IOrderedServiceMapper _orderedServiceMapper;
        private readonly IErrorResponseMapper _errorResponseMapper;
        private readonly IValidator<OrderedServiceRequest> _orderedServiceRequestValidator;

        public OrderedServiceController(IOrderedServiceRepository orderedServiceRepository,
                                        IOrderedServiceMapper orderedServiceMapper,
                                        IErrorResponseMapper errorResponseMapper,
                                        IValidator<OrderedServiceRequest> orderedServiceRequestValidator)
        {
            _orderedServiceRepository = orderedServiceRepository;
            _orderedServiceMapper = orderedServiceMapper;
            _errorResponseMapper = errorResponseMapper;
            _orderedServiceRequestValidator = orderedServiceRequestValidator;
        }

        /// <summary>
        /// Order a service
        /// </summary>
        /// <param name="orderedServiceRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderedServiceResponse>> Post([FromBody] OrderedServiceRequest orderedServiceRequest)
        {
            var validationResponse = GetOrderedServiceResponse(orderedServiceRequest);
            if (validationResponse.ResultCode is ResultConstants.SUCCESS)
            {
                await _orderedServiceRepository.Post(orderedServiceRequest.ServiceId, orderedServiceRequest.UserId, orderedServiceRequest.MechanicId);
                return Ok(validationResponse);
            }
            return BadRequest(validationResponse);
        }

        /// <summary>
        /// Retrieves all services by specified userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<OrderedServiceResponse>> Get([FromQuery] string userId)
        {
            var orderedServices = _orderedServiceRepository.Get(userId);

            if (orderedServices.Count > 0)
            {
                var orderedServicesMapped = _orderedServiceMapper.MapOrderedServices(orderedServices);
                return Ok(orderedServicesMapped);
            }
            else
                return NotFound();
        }

        private OrderedServiceResponse GetOrderedServiceResponse(OrderedServiceRequest orderedServiceRequest)
        {
            OrderedServiceResponse? orderedServiceResponse = null;
            var validationResponse = _orderedServiceRequestValidator.Validate(orderedServiceRequest);
            if (validationResponse is not null && !validationResponse.IsValid)
            {
                List<ResultDetail> resultDetails = _errorResponseMapper.Map(validationResponse);
                orderedServiceResponse = new OrderedServiceResponse
                {
                    ResultCode = ResultConstants.ERROR,
                    ResultDetails = resultDetails
                };
            }
            else
            {
                orderedServiceResponse = new OrderedServiceResponse
                {
                    ResultCode = ResultConstants.SUCCESS
                };
            }
            return orderedServiceResponse;
        }
    }
}
