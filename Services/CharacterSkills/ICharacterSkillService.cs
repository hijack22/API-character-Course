using System.Threading.Tasks;
using API_Course.DTO.Character;
using API_Course.DTO.Skills;
using API_Course.Models;

namespace API_Course.Services.CharacterSkills
{
    public interface ICharacterSkillService
    {
         Task<serviceResponse<GetCharacterDTO>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill);
    }
}