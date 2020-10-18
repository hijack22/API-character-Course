using API_Course.DTO.Character;
using API_Course.Models;
using AutoMapper;

namespace API_Course
{
    public class AutomapperProfile: Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Character, GetCharacterDTO>();
            CreateMap<AddCharacterDTO,Character>();
        }
    }
}