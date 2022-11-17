using CarBom.Mappers;
using CarBom.Requests;
using CarBom.Responses;
using CarBom.Utils;
using DataProvider.DataModels;
using DataProvider.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CarBom.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginRepository _loginRepository;
        private readonly IErrorResponseMapper _errorResponseMapper;
        private readonly IValidator<UserRequest> _userRequestValidator;
        private readonly IValidator<User> _userValidator;

        public LoginController(ILoginRepository loginRepository,
                               IErrorResponseMapper errorResponseMapper,
                               IValidator<UserRequest> userRequestValidator,
                               IValidator<User> userValidator)
        {
            _loginRepository = loginRepository;
            _errorResponseMapper = errorResponseMapper;
            _userRequestValidator = userRequestValidator;
            _userValidator = userValidator;
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("createuser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            var validationResponse = GetUserResponse(user);
            if (validationResponse is null)
            {
                user.Password = EncryptUtil.EncryptToSha256Hash(user.Password);
                await _loginRepository.Post(user);
                return Ok(GetUserResponse(true));
            }
            return BadRequest(validationResponse);
        }

        /// <summary>
        /// Verify if the user credentials are valid
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Return isValid = true if is valid, isValid = false if it is not</returns>
        [HttpPost]
        [Route("authenticate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserResponse>> Post([FromBody] UserRequest user)
        {
            var validationResponse = GetUserResponse(user);
            if (validationResponse is null)
            {
                user.Password = EncryptUtil.EncryptToSha256Hash(user.Password);
                bool isValid = await _loginRepository.Get(user.Email, user.Password);
                return Ok(GetUserResponse(isValid));
            }
            return BadRequest(validationResponse);
        }


        private UserResponse GetUserResponse(User user)
        {
            UserResponse userResponse = null;
            var validationResponse = _userValidator.Validate(user);
            if (validationResponse is not null && !validationResponse.IsValid)
            {
                List<ResultDetail> resultDetails = _errorResponseMapper.Map(validationResponse);
                userResponse = new UserResponse
                {
                    ResultCode = ResultConstants.ERROR,
                    ResultDetails = resultDetails
                };
            }
            return userResponse;
        }

        private UserResponse GetUserResponse(UserRequest user)
        {
            UserResponse userResponse = null;
            var validationResponse = _userRequestValidator.Validate(user);
            if (validationResponse is not null && !validationResponse.IsValid)
            {
                List<ResultDetail> resultDetails = _errorResponseMapper.Map(validationResponse);
                userResponse = new UserResponse
                {
                    ResultCode = ResultConstants.ERROR,
                    ResultDetails = resultDetails
                };
            }
            return userResponse;
        }

        private UserResponse GetUserResponse(bool isValid)
        {
            string resultCode = isValid ? ResultConstants.SUCCESS : ResultConstants.INVALID_CREDENTIALS;
            UserResponse userResponse = new UserResponse
            {
                isValid = isValid,
                ResultCode = resultCode
            };
            return userResponse;
        }
    }
}
