using System;
using System.Security.Claims;
using System.Threading.Tasks;
using API_Course.Data;
using API_Course.DTO.Character;
using API_Course.DTO.Skills;
using API_Course.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace API_Course.Services.CharacterSkills
{
    public class CharacterSkillService : ICharacterSkillService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        public CharacterSkillService(DataContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _mapper = mapper;
           _httpContextAccessor = (HttpContextAccessor)httpContextAccessor;
            _context = context;

        }

        public async Task<serviceResponse<GetCharacterDTO>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill)
        {
                serviceResponse<GetCharacterDTO> response = new serviceResponse<GetCharacterDTO>();

                try
                {
                    Character character = await _context.Characters
                    .Include(c => c.Weapon)
                    .Include(c => c.CharacterSkills).ThenInclude(cs => cs.Skill)
                    .FirstOrDefaultAsync(c => c.Id == newCharacterSkill.CharacterId && 
                    c.User.id == int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));

                    if(character == null)
                    {
                        response.Success = false;
                        response.Message = "Character not found.";
                        return response;
                    }

                    Skill skill = await _context.Skills
                    .FirstOrDefaultAsync( s => s.Id == newCharacterSkill.SkillId);

                    if (skill == null)
                    {
                       response.Success = false;
                       response.Message = "Skills Not Found"; 
                       return response;
                    }

                    CharacterSkill characterSkill = new CharacterSkill
                    {
                        Character = character,
                        Skill = skill
                    };

                    await _context.CharacterSkills.AddAsync(characterSkill);
                    await _context.SaveChangesAsync();

                    response.Data = _mapper.Map<GetCharacterDTO>(character);
                } 
                catch (Exception ex)
                {
                    response.Success = false;
                    response.Message = ex.Message;
                    
                }

                return response;
        }
    }
}