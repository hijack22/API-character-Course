using System.Collections.Generic;
using System.Threading.Tasks;
using API_Course.DTO.Fight;
using API_Course.Models;

namespace API_Course.Services.FightService
{
    public interface IFightService
    {
          Task<serviceResponse<AttackResultDTo>> WeaponAttack(WeaponAttackDTO request);

          Task<serviceResponse<AttackResultDTo>> skillAttack(SkillAttackDTo request);
    
          Task<serviceResponse<FightResultDTO>> Fight(FightRequestDTO request);

          Task<serviceResponse<List<HighScoreDTO>>> HighScore();
    }   
}