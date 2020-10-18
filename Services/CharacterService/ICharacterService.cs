using System.Collections.Generic;
using System.Threading.Tasks;
using API_Course.Models;
using API_Course.Services;
using API_Course.DTO.Character;

namespace API_Course.Services.CharacterService
{
    public interface ICharacterService
    {
        Task <serviceResponse<List<GetCharacterDTO>>> GetAllCharacters();

        Task<serviceResponse<GetCharacterDTO>> GetCharacterById(int id);

         Task<serviceResponse<List<GetCharacterDTO>>> AddCharacter(AddCharacterDTO newCharacter);

         Task<serviceResponse<GetCharacterDTO>> UpdateCharacter(UpdateCharacterDTO updateCharacter); 
    
        Task<serviceResponse<List<GetCharacterDTO>>> DeleteCharacter(int id);
    }
}