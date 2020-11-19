

using System.Collections.Generic;

namespace API_Course.Models
{
    public class Character
    {
        //ss
        public int Id {get; set; }

        public string Name {get; set;} = "Frodo";

        public int HitPoints {get; set; } = 100;
        
        public int Strength  {get; set; } = 10;

         public int Defense  {get; set; } = 10;

         public int Intellegence  {get; set; } = 10;

         public rpgClass Class {get; set; } = rpgClass.Archer;

         public User User {get; set;}

         public WeaponModel Weapon {get;set;}

        public List<CharacterSkill> CharacterSkills { get; set; }
         
    }
}