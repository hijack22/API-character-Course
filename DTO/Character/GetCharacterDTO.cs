
using System.Collections.Generic;
using API_Course.DTO.Skills;
using API_Course.DTO.Weapon;
using API_Course.Models;

namespace API_Course.DTO.Character
{
    public class GetCharacterDTO
    {
         public int Id {get; set; }

        public string Name {get; set;} 

        public int HitPoints {get; set;} 
        
        public int Strength  {get; set;} 
         public int Defense  {get; set;} 

         public int Intellegence  {get; set;} 
         public rpgClass Class {get; set;} 
         
        public GetWeaponDTO Weapon {get; set;}

        public List<GetSkillDTO> Skills {get; set;}

         public int Fights { get; set; }

        public int Defeats { get; set; }

        public int Wins {get; set;}
    }
}

