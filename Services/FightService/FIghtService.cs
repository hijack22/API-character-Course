using System;
using System.Threading.Tasks;
using API_Course.Data;
using API_Course.DTO.Fight;
using API_Course.Models;
using Microsoft.EntityFrameworkCore;

namespace API_Course.Services.FightService
{
    public class FIghtService : IFightService
    {
        private readonly DataContext _context;
        public FIghtService(DataContext context)
        {
            _context = context;

        }

        public async Task<serviceResponse<AttackResultDTo>> WeaponAttack(WeaponAttackDTO request)
        {
            serviceResponse<AttackResultDTo> response = new serviceResponse<AttackResultDTo>();
            try
            {
                    Character attacker = await _context.Characters
                    .Include(c => c.Weapon)
                    .FirstOrDefaultAsync (c => c.Id == request.AttackerId);

                    Character opponent = await _context.Characters
                    .Include(c => c.Weapon)
                    .FirstOrDefaultAsync (c => c.Id == request.OpponentId); 


                    int damage = attacker.Weapon.Damage + (new Random().Next(attacker.Strength));
                    damage -= new Random().Next(opponent.Defense);

                    if( damage > 0)
                    {
                        opponent.HitPoints -= damage;

                    }

                    if(opponent.HitPoints <= 0)
                    {
                        response.Message = "Fighters health is 0 you LOSE";
                        opponent.Defeats = opponent.Defeats + 1;
                    }
                
                    _context.Characters.Update(opponent);
                   await _context.SaveChangesAsync();
            
                response.Data = new AttackResultDTo
                {
                    Attacker = attacker.Name,
                    AttackerHp = attacker.HitPoints,
                    Oppenent = opponent.Name,
                    OpponentHp = opponent.HitPoints,
                    Damage = damage
                };
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