using System.Threading.Tasks;
using API_Course.Data;
using API_Course.DTO.Character;
using API_Course.Models;
using Microsoft.AspNetCore.Mvc;
using API_Course.Data;

namespace API_Course.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _Auth;
        public AuthController(IAuthRepository Auth)
        {
            _Auth = Auth;

        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserDTO request)
        {
            serviceResponse<int> response = await _Auth.Register(
                new User {Username = request.Username}, request.Password
            );

            if(!response.Success)
            {
              return BadRequest(response);
            }
            return Ok(response);
        }
    
         [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDTO request)
        {
            serviceResponse<string> response = await _Auth.Login(
                request.username, request.password
               );

            if(!response.Success)
            {
              return BadRequest(response);
            }
            return Ok(response);
        }
    }
}

