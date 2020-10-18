using API_Course.Models;

namespace API_Course.DTO.Character
{
    public class AddCharacterDTO
    {
      public string Name {get; set;} = "Frodo";

        public int HitPoints {get; set; } = 100;
        
        public int Strength  {get; set; } = 10;

         public int Defense  {get; set; } = 10;

         public int Intellegence  {get; set; } = 10;

         public rpgClass Class {get; set; } = rpgClass.Archer;
    }
}
    
