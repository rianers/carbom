using CarBom.DTO;
using CarBom.Utils;
using DataProvider.DataModels;
using DataProvider.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CarBom.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginRepository _loginRepository;

        public LoginController(ILoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }

        [HttpPost]
        [Route("createuser")]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            if (user is not null)
            {
                try
                {
                    user.Password = EncryptUtil.EncryptToSha256Hash(user.Password);
                    await _loginRepository.Post(user);
                }
                catch (Exception)
                {
                    return BadRequest();
                }
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("authenticate")]
        public async Task<IActionResult> Post([FromBody] UserDTO user)
        {
            if (user.Email is not null && user.Password is not null)
            {
                try
                {
                    user.Password = EncryptUtil.EncryptToSha256Hash(user.Password);
                    bool isValid = await _loginRepository.Get(user.Email, user.Password);
                    return Ok(isValid);
                }
                catch (Exception)
                {
                    return BadRequest();
                }
            }
            return BadRequest();
        }
    }
}
