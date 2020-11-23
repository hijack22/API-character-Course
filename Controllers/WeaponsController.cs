using System.Threading.Tasks;
using API_Course.DTO.Weapon;
using API_Course.Services.Weapons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Course.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WeaponsController : ControllerBase
    {
        private readonly IWeapons _weaponService;

        public WeaponsController(IWeapons weaponService)
    
        {
            _weaponService = weaponService;
        }
        [HttpPost]
        public async Task<IActionResult> AddWeapon(WeaponDTO newWeapon)
        {
            return Ok(await _weaponService.AddWeapon(newWeapon));
        }
    }
}