using Microsoft.AspNetCore.Mvc;
using API_Course.Models;

namespace API_Course.Controllers
{
    [ApiController]
    [Route("controller")]
    public class CharacterController: ControllerBase
     {
       private static Character Knight = new Character();

        [HttpGet]
        public IActionResult Get() {

            return Ok(Knight);


         }

      
    }
}