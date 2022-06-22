using SharpEcho.CodeChallenge.Api.Team.Entities;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SharpEcho.CodeChallenge.Client
{
    internal class Program
    {
        static HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
            Main().GetAwaiter().GetResult();
        }

        static async Task Main()
        {
            Team team1;
            Team team2;
            try
            {
                try
                {
                    team1 = await AddTeam("Dallas Cowboys");
                }
                catch (Exception _)
                {
                    team1 = await GetTeamByName("Dallas Cowboys");
                    Console.WriteLine("Dallas Cowboys is an existing team!");
                }
                try
                {
                    team2 = await AddTeam("Atlanta Falcons");
                }
                catch (Exception _)
                {
                    team2 = await GetTeamByName("Atlanta Falcons");
                    Console.WriteLine("Atlanta Falcons is an existing team!");
                }
            } catch (Exception _)
            {
                Console.WriteLine("Can't connect!");
                return;
            }
            for (int i = 0; i < 17; i++)
                await RecordMatch(team1, team2, true);
            for (int i = 0; i < 11; i++)
                await RecordMatch(team1, team2, false);

            Tuple<int, int> data = await GetWinLoss(team1, team2);
            Console.WriteLine($"Dallas Cowboys played with Atlanta Falcons {data.Item1 + data.Item2} times and wins {data.Item1} times!");
        }


        static async Task<Team> AddTeam(string name)
        {
            Team team = new Team()
            {
                Name = name
            };
            HttpContent content = new StringContent(JsonConvert.SerializeObject(team), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("https://localhost:5001/api/Teams", content);
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<Team>(await response.Content.ReadAsStringAsync());
        }

        static async Task<Team> GetTeamByName(string name)
        {
            HttpResponseMessage response = await client.GetAsync($"https://localhost:5001/api/Teams/GetTeamByName?name={name}");
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<Team>(await response.Content.ReadAsStringAsync());
        }

        static async Task<Match> RecordMatch(Team team1, Team team2, bool firstWin)
        {
            Match match = new Match()
            {
                FirstTeamId = team1.Id,
                SecondTeamId = team2.Id,
                FirstTeamWins = firstWin
            };
            HttpContent content = new StringContent(JsonConvert.SerializeObject(match), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("https://localhost:5001/api/Matches", content);
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<Match>(await response.Content.ReadAsStringAsync());
        }

        static async Task<Tuple<int, int>> GetWinLoss(Team team1, Team team2)
        {
            HttpResponseMessage response = await client.GetAsync($"https://localhost:5001/api/Matches/GetOverallWinLoss?team1={team1.Id}&team2={team2.Id}");
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<Tuple<int, int>>(await response.Content.ReadAsStringAsync());
        }
    }
}
