namespace API_Course.DTO.Fight
{
    public class AttackResultDTo
    {
        public string Attacker { get; set; }

        public string Oppenent { get; set; }

        public int AttackerHp { get; set; }

        public int OpponentHp {get; set;}

        public int Damage { get; set; }
    }
}