using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpEcho.CodeChallenge.Api.Team.Controllers;
using SharpEcho.CodeChallenge.Data;
using System;

namespace SharpEcho.CodeChallenge.Api.Team.Tests
{
    [TestClass]
    public class TeamUnitTests
    {
        IRepository Repository = new GenericRepository(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("SharpEcho"));

        [TestMethod]
        public void GetTeamByName_ShouldReturnCorrectTeam()
        {
            var controller = new TeamsController(Repository);

            var team = new Entities.Team
            {
                Name = "Houston Cougars"
            };

            var result = controller.GetTeamByName(team.Name).Value;

            if (result == null)
            {
                controller.Post(team);
                result = controller.GetTeamByName(team.Name).Value;
            }

            Assert.AreEqual(team.Name, result.Name);
        }

        [TestMethod]
        public void GetTeamByName_ShouldNotReturnTeam()
        {
            var controller = new TeamsController(Repository);

            var team = new Entities.Team
            {
                Name = Guid.NewGuid().ToString()
            };

            var result = controller.GetTeamByName(team.Name).Value;

            Assert.IsNull(result);
        }

        [TestMethod]
        public void PostMatch()
        {
            var matchesController = new MatchesController(Repository);
            var teamsController = new TeamsController(Repository);
            var team1 = new Entities.Team
            {
                Name = "Houston Cougars"
            };
            var team2 = new Entities.Team
            {
                Name = "Pandas Hut"
            };
            var resultTeam1 = teamsController.GetTeamByName(team1.Name).Value;

            if (resultTeam1 == null)
            {
                teamsController.Post(team1);
                resultTeam1 = teamsController.GetTeamByName(team1.Name).Value;
            }
            var resultTeam2 = teamsController.GetTeamByName(team2.Name).Value;

            if (resultTeam2 == null)
            {
                teamsController.Post(team2);
                resultTeam2 = teamsController.GetTeamByName(team2.Name).Value;
            }

            var match = new Entities.Match
            {
                FirstTeamId = resultTeam1.Id,
                SecondTeamId = resultTeam2.Id,
                FirstTeamWins = true,
            };

            matchesController.Post(match);
            var resultMatch = matchesController.Get(match.Id).Value;

            Assert.AreEqual(match.FirstTeamId, resultMatch.FirstTeamId);
            Assert.AreEqual(match.SecondTeamId, resultMatch.SecondTeamId);
            Assert.AreEqual(match.FirstTeamWins, resultMatch.FirstTeamWins);
            Assert.AreEqual(match.Id, resultMatch.Id);
        }

        [TestMethod]
        public void GetOverallWinLoss()
        {
            var matchesController = new MatchesController(Repository);
            var teamsController = new TeamsController(Repository);
            var team1 = new Entities.Team
            {
                Name = "Houston Cougars"
            };
            var team2 = new Entities.Team
            {
                Name = "Pandas Hut"
            };
            var resultTeam1 = teamsController.GetTeamByName(team1.Name).Value;

            if (resultTeam1 == null)
            {
                teamsController.Post(team1);
                resultTeam1 = teamsController.GetTeamByName(team1.Name).Value;
            }
            var resultTeam2 = teamsController.GetTeamByName(team2.Name).Value;

            if (resultTeam2 == null)
            {
                teamsController.Post(team2);
                resultTeam2 = teamsController.GetTeamByName(team2.Name).Value;
            }

            var match = new Entities.Match
            {
                FirstTeamId = resultTeam1.Id,
                SecondTeamId = resultTeam2.Id,
                FirstTeamWins = true,
            };

            match = matchesController.Post(match).Value;

            var result = matchesController.GetOverallWinLoss(resultTeam1.Id, resultTeam2.Id).Value;
            var matches = matchesController.Get();
            int wins = 0;
            int losses = 0;
            foreach (var m in matches)
            {
                if (m.FirstTeamId != resultTeam1.Id && m.FirstTeamId != resultTeam2.Id) continue;
                if (m.SecondTeamId != resultTeam1.Id && m.SecondTeamId != resultTeam2.Id) continue;
                if (m.FirstTeamId == resultTeam1.Id && m.FirstTeamWins) wins++;
                else if (m.SecondTeamId == resultTeam1.Id && !m.FirstTeamWins) wins++;
                else losses++;
            }
            Assert.AreEqual(result, new Tuple<int, int>(wins, losses));
        }
    }
}
