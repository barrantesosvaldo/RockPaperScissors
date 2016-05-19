using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RockPaperScissors.Models
{
    public class Tournament
    {
        public List<Player> ListOfPlayers { get; set; }
        public List<Tournament> ListOfTournaments { get; set; }

        public Tournament()
        {
            ListOfPlayers = new List<Player>();
            ListOfTournaments = new List<Tournament>();
        }
    }
}
