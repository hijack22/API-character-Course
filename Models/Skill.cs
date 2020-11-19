using System.Collections.Generic;

namespace API_Course.Models
{
    public class Skill
    {
        public int Id { get; set; }

        public string Name {get; set;}

        public int Damage { get; set; }

        public List<CharacterSkill> CharacterSkills { get; set; }
    }
}