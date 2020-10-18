using Microsoft.AspNetCore.Mvc;
using API_Course.Models;
using System.Collections.Generic;
using System.Linq;
using API_Course.Services.CharacterService;
using System.Threading.Tasks;
using API_Course.DTO.Character;

namespace API_Course.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterService _characterService;
        public CharacterController(ICharacterService characterService)
        {
            this._characterService = characterService;

        }
        private static List<Character> characters = new List<Character>
         {
            new Character(),
            new Character {Id = 1, Name = "Sam"}

         };
        [HttpGet("GetAll")]
        public async Task <IActionResult> Get()
        {

            return Ok(await _characterService.GetAllCharacters());
        }

        [HttpGet("{id}")]
        public async Task <IActionResult> GetSingle(int id)
        {
            return Ok( await _characterService.GetCharacterById(id));
        }

        [HttpPost]
        public async Task <IActionResult> AddCharacter(AddCharacterDTO newCharacter)
        {
            return Ok( await _characterService.AddCharacter(newCharacter));

        }
   
      [HttpPut]
      public async Task<IActionResult> UpdateCharacter(UpdateCharacterDTO updateCharacter)
      {
        serviceResponse<GetCharacterDTO> response =  await _characterService.UpdateCharacter(updateCharacter);
        
        if(response.Data == null)
        {
            return NotFound(response);

        }
        
        return Ok(response);

      
      }
      [HttpDelete("{id}")]
        public async Task <IActionResult> Delete(int id)
        {
            serviceResponse<List<GetCharacterDTO>> response =  await _characterService.DeleteCharacter(id);

             if(response.Data == null)
          {
            return NotFound(response);

          }
        
        return Ok(response);
            
        }

    }

}