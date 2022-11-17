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
    public class MechanicController : ControllerBase
    {
        private readonly IMechanicRepository _mechanicRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IErrorResponseMapper _errorResponseMapper;
        private readonly IValidator<MechanicRequest> _mechanicRequestValidator;
        public MechanicController(IMechanicRepository mechanicRepository,
                                  IAddressRepository addressRepository,
                                  IErrorResponseMapper errorResponseMapper,
                                  IValidator<MechanicRequest> mechanicRequestValidator)
        {
            _mechanicRepository = mechanicRepository;
            _addressRepository = addressRepository;
            _errorResponseMapper = errorResponseMapper;
            _mechanicRequestValidator = mechanicRequestValidator;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<Mechanic>> Get([FromQuery] MechanicListRequest mechanicListDTO, [FromServices] IMechanicMapper mechanicMapper)
        {
            List<Mechanic>? mechanicsResponse = _mechanicRepository.GetAll();

            var mechanics = mechanicMapper.MapMechanics(mechanicsResponse, mechanicListDTO.UserLatitude, mechanicListDTO.UserLongitude).AsQueryable();

            if (mechanicListDTO.Id is not null)
                mechanics = mechanics.Where(m => m.Id.Contains(mechanicListDTO.Id));

            if (mechanicListDTO.Name is not null)
                mechanics = mechanics.Where(m => m.Name.ToUpper().Contains(mechanicListDTO.Name.ToUpper()));

            if (mechanicListDTO.Services is not null)
                mechanics = mechanics.Where(m => m.Services.Where(s => mechanicListDTO.Services.Contains(s.Name)).Any());

            return mechanics.Any() ? Ok(mechanics) : NotFound();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MechanicResponse>> Post([FromBody] MechanicRequest mechanicRequest)
        {
            var validationResponse = GetMechanicResponse(mechanicRequest);
            if (validationResponse.ResultCode is ResultConstants.SUCCESS)
            {
                Mechanic mechanic = new Mechanic
                {
                    Name = mechanicRequest.Name,
                    Description = mechanicRequest.Description,
                    Address = mechanicRequest.Address,
                    Image = mechanicRequest.Image,
                    Ranking = mechanicRequest.Ranking
                };

                int mechanicId = await _mechanicRepository.Post(mechanic);
                await _addressRepository.Post(mechanic.Address, mechanicId.ToString());
                return Ok(validationResponse);
            }
            return BadRequest(validationResponse);
        }

        private MechanicResponse GetMechanicResponse(MechanicRequest mechanicRequest)
        {
            MechanicResponse? mechanicResponse = null;
            var validationResponse = _mechanicRequestValidator.Validate(mechanicRequest);
            if (validationResponse is not null && !validationResponse.IsValid)
            {
                List<ResultDetail> resultDetails = _errorResponseMapper.Map(validationResponse);
                mechanicResponse = new MechanicResponse
                {
                    ResultCode = ResultConstants.ERROR,
                    ResultDetails = resultDetails
                };
            }
            else
            {
                mechanicResponse = new MechanicResponse
                {
                    ResultCode = ResultConstants.SUCCESS
                };
            }
            return mechanicResponse;
        }
    }
}
