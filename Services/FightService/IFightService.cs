using System.Threading.Tasks;
using API_Course.DTO.Fight;
using API_Course.Models;

namespace API_Course.Services.FightService
{
    public interface IFightService
    {
          Task<serviceResponse<AttackResultDTo>> WeaponAttack(WeaponAttackDTO request);
    }   
}