using Microsoft.AspNetCore.Mvc;
using SharpEcho.CodeChallenge.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpEcho.CodeChallenge.Api.Team.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchesController : GenericController<Entities.Match>
    {
        public MatchesController(IRepository repository) : base(repository)
        {
        }

        [HttpGet("GetOverallWinLoss")]
        public virtual ActionResult<Tuple<int, int>> GetOverallWinLoss(long team1, long team2)
        {
            List<Entities.Match> matches = Repository.Query<Entities.Match>("SELECT * FROM Match WHERE (FirstTeamId = @Team1 or SecondTeamId = @Team1) and (FirstTeamId = @Team2 or SecondTeamId = @Team2)", new { Team1 = team1, Team2 = team2 }).ToList();
            if (matches == null)
                return NotFound();
            int wins = 0;
            foreach (Entities.Match match in matches)
            {
                if (match.FirstTeamWins && match.FirstTeamId == team1) wins++;
                else if (!match.FirstTeamWins && match.SecondTeamId == team1) wins++;
            }
            return new Tuple<int, int>(wins, matches.Count - wins);
        }
    }
}
