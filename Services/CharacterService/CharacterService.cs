using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Course.Models;
using API_Course.DTO.Character;
using AutoMapper;
using System;
using API_Course.Data;
using Microsoft.EntityFrameworkCore;

namespace API_Course.Services.CharacterService
{

    public class CharacterService : ICharacterService
    {
    


    
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public CharacterService(IMapper mapper, DataContext context)
        {
            _context = context;
            _mapper = mapper;

        }
        public async Task<serviceResponse<List<GetCharacterDTO>>> AddCharacter(AddCharacterDTO newCharacter)
        {
            serviceResponse<List<GetCharacterDTO>> serviceResponse = new serviceResponse<List<GetCharacterDTO>>();
            Character character = _mapper.Map<Character>(newCharacter);
            //Adds new character into db using AddAsync and then Save changes
            await _context.Characters.AddAsync(character);
            await _context.SaveChangesAsync();
            //Return Data to web server
            serviceResponse.Data = (_context.Characters.Select(c => _mapper.Map<GetCharacterDTO>(c))).ToList();
            return serviceResponse;
        }

        public async Task<serviceResponse<List<GetCharacterDTO>>> DeleteCharacter(int id)
        {
            serviceResponse<List<GetCharacterDTO>> serviceResponse = new serviceResponse<List<GetCharacterDTO>>();

            try
            {
                
                Character dbCharacter =  await _context.Characters.FirstAsync(c => c.Id == id);
               _context.Characters.Remove(dbCharacter);
               await _context.SaveChangesAsync();

                serviceResponse.Data = (_context.Characters.Select(c => _mapper.Map<GetCharacterDTO>(c))).ToList();

            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;

                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }


        public async Task<serviceResponse<List<GetCharacterDTO>>> GetAllCharacters(int userId)
        {
            serviceResponse<List<GetCharacterDTO>> serviceResponse = new serviceResponse<List<GetCharacterDTO>>();
            //Get Characters from the database
            List<Character> dbCharacters = await _context.Characters.Where(c => c.User.id == userId).ToListAsync();
            
            //Grab Characters list from the database and then display to web server
            serviceResponse.Data = (dbCharacters.Select(c => _mapper.Map<GetCharacterDTO>(c))).ToList();

            return serviceResponse;
        }

        public Task<object> GetAllCharacters(object userId)
        {
            throw new NotImplementedException();
        }

        public async Task<serviceResponse<GetCharacterDTO>> GetCharacterById(int id)
        {
            serviceResponse<GetCharacterDTO> serviceResponse = new serviceResponse<GetCharacterDTO>();
            Character dbCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);
            serviceResponse.Data = _mapper.Map<GetCharacterDTO>(dbCharacter);

            return serviceResponse;
        }

        public async Task<serviceResponse<GetCharacterDTO>> UpdateCharacter(UpdateCharacterDTO updateCharacter)
        {
            serviceResponse<GetCharacterDTO> serviceResponse = new serviceResponse<GetCharacterDTO>();
            try
            {
                Character character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == updateCharacter.Id);
                character.Name = updateCharacter.Name;
                character.Class = updateCharacter.Class;
                character.Defense = updateCharacter.Defense;
                character.HitPoints = updateCharacter.HitPoints;
                character.Intellegence = updateCharacter.Intellegence;
                character.Strength = updateCharacter.Strength;
                
                _context.Characters.Update(character);
                await _context.SaveChangesAsync();
                serviceResponse.Data = _mapper.Map<GetCharacterDTO>(character);

            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;

                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
    }
}