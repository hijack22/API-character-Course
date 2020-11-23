using System.Threading.Tasks;
using API_Course.DTO.Character;
using API_Course.DTO.Weapon;
using API_Course.Models;

namespace API_Course.Services.Weapons
{
    public interface IWeapons
    {
         Task<serviceResponse<GetCharacterDTO>> AddWeapon(WeaponDTO newWeapon);
    }
}