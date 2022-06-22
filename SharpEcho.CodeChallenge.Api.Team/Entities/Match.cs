namespace SharpEcho.CodeChallenge.Api.Team.Entities
{
    public class Match
    {
        public long Id { get; set; }

        public long FirstTeamId { get; set; }

        public long SecondTeamId { get; set; }

        public bool FirstTeamWins { get; set; }
    }
}
