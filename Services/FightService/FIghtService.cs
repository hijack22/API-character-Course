using System;
using System.Collections.Generic;
using System.Linq;
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

        public  async Task<serviceResponse<FightResultDTO>> Fight(FightRequestDTO request)
        {
            serviceResponse<FightResultDTO> response = new serviceResponse<FightResultDTO>
            {
                Data = new FightResultDTO()
            };
            
            try
            {
                List<Character> Character =

                await _context.Characters
                .Include(c => c.Weapon)
                .Include(c => c.CharacterSkills).ThenInclude(c => c.Skill)
                .Where(c => request.CharacterIds.Contains(c.Id)).ToListAsync();
                
                bool defeated = false;
                while(!defeated)
                {
                    foreach(Character attacker in Character)
                    {
                        List<Character> opponents = Character.Where(c => c.Id != attacker.Id).ToList();
                        Character opponent = opponents[new Random().Next(opponents.Count)];

                        int damage = 0;
                        string attackUsed = string.Empty;

                        bool useWeapon = new Random().Next(2) == 0;

                        if(useWeapon)
                        {
                           attackUsed = attacker.Weapon.Name;
                           damage = DoWeaponAttack(attacker, opponent);
                        }
                        else
                        {
                            int randomSkill = new Random().Next(attacker.CharacterSkills.Count);
                            attackUsed = attacker.CharacterSkills[randomSkill].Skill.Name;
                            damage = SkillAttack(attacker,  opponent, attacker.CharacterSkills[randomSkill]);
                        }
                            response.Data.Log.Add($"{attacker.Name} attacked {opponent.Name} with {attackUsed} with damage {(damage >=0 ? damage : 0)} damage.");

                        if(opponent.HitPoints <= 0)
                        {
                            defeated = true;
                            attacker.Wins++;
                            opponent.Defeats++;
                            response.Data.Log.Add($"{opponent.Name} was defeated by {attacker.Name} ");
                            break;
                        }
                    }
                }
            
                Character.ForEach(c => 
                {
                    c.Fights++; 
                    c.HitPoints = 100;

                });

                _context.Characters.UpdateRange(Character);
                 await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                
            }

            return response;
        }

        public async Task<serviceResponse<AttackResultDTo>> skillAttack(SkillAttackDTo request)
        {
            serviceResponse<AttackResultDTo> response = new serviceResponse<AttackResultDTo>();
            
            try
            {
                Character attacker = await _context.Characters
                .Include(c => c.CharacterSkills).ThenInclude(cs => cs.Skill)
                .FirstOrDefaultAsync(c => c.Id == request.AttackerId);


                Character opponent = await _context.Characters
                .FirstOrDefaultAsync(c => c.Id == request.OpponentId);



                CharacterSkill character = attacker.CharacterSkills.FirstOrDefault(cs => cs.Skill.Id == request.SkillId);

                if (character == null)
                {
                    response.Success = false;
                    response.Message = $" {attacker.Name} does not know that skill.";
                }

                int damage = SkillAttack(attacker, opponent, character);

                if (opponent.HitPoints <= 0)
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

            catch (Exception ex )
            {
                response.Success = false;
                response.Message = ex.Message;
                
            }

            return response;
        }

        private static int SkillAttack(Character attacker, Character opponent, CharacterSkill character)
        {
            int damage = character.Skill.Damage + (new Random().Next(attacker.Intellegence));
            damage -= new Random().Next(opponent.Defense);

            if (damage > 0)
            {
                opponent.HitPoints -= damage;

            }

            return damage;
        }

        public async Task<serviceResponse<AttackResultDTo>> WeaponAttack(WeaponAttackDTO request)
        {
            serviceResponse<AttackResultDTo> response = new serviceResponse<AttackResultDTo>();
            try
            {
                Character attacker = await _context.Characters
                .Include(c => c.Weapon)
                .FirstOrDefaultAsync(c => c.Id == request.AttackerId);

                Character opponent = await _context.Characters
                .Include(c => c.Weapon)
                .FirstOrDefaultAsync(c => c.Id == request.OpponentId);

                int damage = DoWeaponAttack(attacker, opponent);

                if (opponent.HitPoints <= 0)
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

        private static int DoWeaponAttack(Character attacker, Character opponent)
        {
            int damage = attacker.Weapon.Damage + (new Random().Next(attacker.Strength));
            damage -= new Random().Next(opponent.Defense);

            if (damage > 0)
            {
                opponent.HitPoints -= damage;

            }

            return damage;
        }
    }
}
    