using ClientManagement.Models;
using ClientManagement.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClientManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientLoginController : ControllerBase
    {
        private readonly IClientLoginRepository _clientLoginRepository;

        public ClientLoginController(IClientLoginRepository clientLoginRepository)
        {
            _clientLoginRepository = clientLoginRepository;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpModel signUpModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _clientLoginRepository.SignUpAsync(signUpModel);

            if (result != "User signup successful")
            {
                return BadRequest(new { Error = result });
            }
            else
            {
                return Ok(new { Message = result });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] SignInModel signInModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var token = await _clientLoginRepository.LoginAsync(signInModel);

            if (token == "Login attempt is not valid")
            {
                return Unauthorized(new { Error = token });
            }
            else
            {
                return Ok(new { Token = token });
            }
        }
    }
}
