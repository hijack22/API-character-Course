using System;
using System.Security.Claims;
using System.Threading.Tasks;
using API_Course.Data;
using API_Course.DTO.Character;
using API_Course.DTO.Weapon;
using API_Course.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace API_Course.Services.Weapons
{
    public class Weapons : IWeapons
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        
        public Weapons(DataContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _mapper = mapper;
            _httpContextAccessor = (HttpContextAccessor)httpContextAccessor;
            _context = context;

        }
        public async Task<serviceResponse<GetCharacterDTO>> AddWeapon(WeaponDTO newWeapon)
        {
            serviceResponse<GetCharacterDTO> response = new serviceResponse<GetCharacterDTO>();
            try
            {
                Character character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == newWeapon.CharacterId && 
                c.User.id == int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));

                if(character == null)
                {
                    response.Success = false;
                    response.Message = "No Character was found";
                }

                 WeaponModel Weapons = new WeaponModel  
                {
                    Name = newWeapon.Name,
                    Damage = newWeapon.Damage,
                    Character = character

                };

                  await _context.Weapons.AddAsync(Weapons);
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