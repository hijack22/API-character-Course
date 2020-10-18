using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Course.Models;
using API_Course.DTO.Character;
using AutoMapper;
using System;

namespace API_Course.Services.CharacterService
{

    public class CharacterService : ICharacterService
    {
        private static List<Character> characters = new List<Character>
         {
            new Character(),
            new Character {Id = 1, Name = "Sam"}
        

    };
    private readonly IMapper _mapper;

    public CharacterService(IMapper mapper)
    {
       _mapper = mapper;

    }
    public async Task<serviceResponse<List<GetCharacterDTO>>> AddCharacter(AddCharacterDTO newCharacter)
    {
        serviceResponse<List<GetCharacterDTO>> serviceResponse = new serviceResponse<List<GetCharacterDTO>>();
        Character character =  _mapper.Map<Character>(newCharacter);
        character.Id = characters.Max(c => c.Id ) + 1;
        characters.Add(character);
        serviceResponse.Data = (characters.Select(c => _mapper.Map<GetCharacterDTO>(c))).ToList();
        return serviceResponse;
    }

        public async Task<serviceResponse<List<GetCharacterDTO>>> DeleteCharacter(int id)
        {
             serviceResponse<List<GetCharacterDTO>> serviceResponse = new serviceResponse<List<GetCharacterDTO>>();
         
           try {
           Character character = characters.First(c => c.Id == id);
            characters.Remove(character); 
          

            serviceResponse.Data = (characters.Select(c => _mapper.Map<GetCharacterDTO>(c))).ToList();

             } 
           catch(Exception ex)
            {
                serviceResponse.Success = false;
                
                serviceResponse.Message = ex.Message;
             }
            return serviceResponse;
        }
        

        public async Task<serviceResponse<List<GetCharacterDTO>>> GetAllCharacters()
    {
        serviceResponse<List<GetCharacterDTO>> serviceResponse = new serviceResponse<List<GetCharacterDTO>>();
       serviceResponse.Data = (characters.Select(c => _mapper.Map<GetCharacterDTO>(c))).ToList();

        return serviceResponse;
    }

    public async Task<serviceResponse<GetCharacterDTO>> GetCharacterById(int id)
    {
        serviceResponse<GetCharacterDTO> serviceResponse = new serviceResponse<GetCharacterDTO>();
        serviceResponse.Data = _mapper.Map<GetCharacterDTO>(characters.FirstOrDefault(c => c.Id == id));

        return serviceResponse;
    }

        public async Task<serviceResponse<GetCharacterDTO>> UpdateCharacter(UpdateCharacterDTO updateCharacter)
        {
           serviceResponse<GetCharacterDTO> serviceResponse = new serviceResponse<GetCharacterDTO>();
           try {
           Character character = characters.FirstOrDefault(c => c.Id == updateCharacter.Id);
           character.Name = updateCharacter.Name;
           character.Class = updateCharacter.Class;
           character.Defense = updateCharacter.Defense;
           character.HitPoints = updateCharacter.HitPoints;
           character.Intellegence = updateCharacter.Intellegence;
           character.Strength = updateCharacter.Strength;

            serviceResponse.Data = _mapper.Map<GetCharacterDTO>(character);

           } 
           catch(Exception ex)
            {
                serviceResponse.Success = false;
                
                serviceResponse.Message = ex.Message;
             }
            return serviceResponse;
        }
    }
}