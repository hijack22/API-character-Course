using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Course.Models;
using API_Course.DTO.Character;
using AutoMapper;
using System;
using API_Course.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace API_Course.Services.CharacterService
{

    public class CharacterService : ICharacterService
    {
    

                //Test
    
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly HttpContextAccessor _httpContextAccessor;

        public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = (HttpContextAccessor)httpContextAccessor;
            _mapper = mapper;

        }

        private int  GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        public async Task<serviceResponse<List<GetCharacterDTO>>> AddCharacter(AddCharacterDTO newCharacter)
        {
            serviceResponse<List<GetCharacterDTO>> serviceResponse = new serviceResponse<List<GetCharacterDTO>>();
            Character character = _mapper.Map<Character>(newCharacter);
            //Adds new character into db using AddAsync and then Save changes
           character.User = await _context.Users.FirstOrDefaultAsync(u => u.id == GetUserId());
            await _context.Characters.AddAsync(character);
            await _context.SaveChangesAsync();
            //Return Data to web server
            serviceResponse.Data = (_context.Characters.Where(C => C.User.id ==GetUserId()).Select(c => _mapper.Map<GetCharacterDTO>(c))).ToList();
            return serviceResponse;
        }

        public async Task<serviceResponse<List<GetCharacterDTO>>> DeleteCharacter(int id)
        {
            serviceResponse<List<GetCharacterDTO>> serviceResponse = new serviceResponse<List<GetCharacterDTO>>();

            try
            {
                
                Character dbCharacter =  await _context.Characters.FirstOrDefaultAsync(c => c.Id == id && c.User.id == GetUserId());
              
              if(dbCharacter != null)
              {
               _context.Characters.Remove(dbCharacter);
               await _context.SaveChangesAsync();
               serviceResponse.Data = (_context.Characters.Where(C => C.User.id ==GetUserId())
               .Select(c => _mapper.Map<GetCharacterDTO>(c))).ToList();
             
              }
               else
               {
                   serviceResponse.Success = false;
                   serviceResponse.Message = "Character Not Found";
               }

            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;

                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }


        public async Task<serviceResponse<List<GetCharacterDTO>>> GetAllCharacters()
        {
            serviceResponse<List<GetCharacterDTO>> serviceResponse = new serviceResponse<List<GetCharacterDTO>>();
            //Get Characters from the database
            List<Character> dbCharacters = await _context.Characters.Where(c => c.User.id == GetUserId()).ToListAsync();
            
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
            Character dbCharacter = await _context.Characters.
            FirstOrDefaultAsync(c => c.Id == id && c.User.id == GetUserId());
            serviceResponse.Data = _mapper.Map<GetCharacterDTO>(dbCharacter);

            return serviceResponse;
        }

        public async Task<serviceResponse<GetCharacterDTO>> UpdateCharacter(UpdateCharacterDTO updateCharacter)
        {
            serviceResponse<GetCharacterDTO> serviceResponse = new serviceResponse<GetCharacterDTO>();
            try
            {
                Character character = await _context.Characters.Include(c => c.User).FirstOrDefaultAsync(c => c.Id == updateCharacter.Id);
               
                if(updateCharacter.Id == GetUserId())
                {
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
                else {

                    serviceResponse.Success = false;
                    serviceResponse.Message = "User not found";

                }

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