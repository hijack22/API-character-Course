using System.Threading.Tasks;
using API_Course.DTO.Fight;
using API_Course.Services.FightService;
using Microsoft.AspNetCore.Mvc;

namespace API_Course.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class FightController : ControllerBase
    {
        
        private readonly IFightService _fightService;

        public FightController(IFightService fightService)
        {
            _fightService = fightService;
            
            
        }

        [HttpPost("WeaponAttack")]
        public async Task<IActionResult> WeaponAttack(WeaponAttackDTO request)
        {
            return Ok(await _fightService.WeaponAttack(request));
        }
   
    }
}