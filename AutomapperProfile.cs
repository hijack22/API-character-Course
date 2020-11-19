using System.Linq;
using API_Course.DTO.Character;
using API_Course.DTO.Skills;
using API_Course.DTO.Weapon;
using API_Course.Models;

using AutoMapper;

namespace API_Course
{
    public class AutomapperProfile: Profile
    {
        public AutomapperProfile()
        {
            
            
            CreateMap<Character, GetCharacterDTO>()
            .ForMember(dto => dto.Skills, c => c.MapFrom(c => c.CharacterSkills.Select(cs => cs.Skill)));
           
            CreateMap<AddCharacterDTO,Character>();
             
            CreateMap<WeaponModel,GetWeaponDTO>();

            CreateMap<Skill,GetSkillDTO>();
        }
    }
}