namespace API_Course.DTO.Fight
{
    public class HighScoreDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Fights { get; set; }

        public int Wins { get; set; }

        public int Defeats {get; set;}
    }
}