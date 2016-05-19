using RockPaperScissors.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Data.Entity;

namespace RockPaperScissors.Controllers
{
    public class ChampionshipController : ApiController
    {
        private RockPaperScissorsContext db = new RockPaperScissorsContext();
        private Utilities util = new Utilities();

        // GET api/championship/top
        [System.Web.Http.AcceptVerbs("GET")]
        [System.Web.Http.HttpGet]
        public string Top(int? count)
        {
            int n;
            if (count == null)
            {
                n = 10;
            }
            else
            {
                n = (int)count;
            }

            List<Player> listOfPlayers = db.Player.ToList();

            if ((listOfPlayers.ToList().Count == 0) || (n == 0))
            {
                return "players: []";
            }
            else
            {
                listOfPlayers = listOfPlayers.Take(n).ToList();
            }


            string result = "players: [";
            foreach (Player player in listOfPlayers)
            {
                result += "\"" + player.Name + "\", ";
            }
            result = result.Remove(result.Length - 2);

            result += "]";
            return result;
        }

        // POST api/values
        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Http.HttpPost]
        public string Result()
        {
            try
            {
                string fileContents = System.IO.File.ReadAllText(
                    HttpContext.Current.Server.MapPath(
                        @"~/App_Data/UploadedFiles/championship.txt"));
                fileContents = fileContents.Replace(" ", string.Empty);
                string tempTournament = fileContents;
                Tournament tournament = util.stringToTournament(ref tempTournament);
                List<Player> players = util.getFirstAndSecondPlace(tournament);

                storeAndScore(players);
                
                return "first=" + players.ElementAt(0).Name + 
                    "&second=" + players.ElementAt(1).Name;
            }
            catch(Exception ex)
            {
                Console.Write(ex.Message);
                return "Something went wrong.";
            }
        }

        /// <summary>
        /// Stores the players in the Database with 
        /// the scores updated.
        /// </summary>
        /// <param name="players"> First and Second Players</param>
        private void storeAndScore(List<Player> players)
        {
            Player first = players.ElementAt(0);
            Player second = players.ElementAt(1);
            List<Player> temp;

            // First Player
            temp = db.Player.Where(pl => pl.Name.ToUpper() == first.Name.ToUpper()).ToList();

            if (temp.Count == 0)
            {
                first.Points = 3;
                db.Player.Add(first);
            }
            else
            {
                temp.ElementAt(0).Points += 3;
                db.Entry(temp.ElementAt(0)).State = EntityState.Modified;
            }
            db.SaveChanges();

            // Second Player
            temp = db.Player.Where(pl => pl.Name.ToUpper() == second.Name.ToUpper()).ToList();

            if (temp.Count == 0)
            {
                second.Points = 1;
                db.Player.Add(second);
            }
            else
            {
                temp.ElementAt(0).Points += 1;
                db.Entry(temp.ElementAt(0)).State = EntityState.Modified;
            }
            db.SaveChanges();
        }
    }
}
