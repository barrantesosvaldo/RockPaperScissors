using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RockPaperScissors.Models;
using RockPaperScissors.CustomExceptions;

namespace RockPaperScissors.Controllers
{
    public class Utilities
    {
        // Array of Strategies
        private string[] _strategies = { "R", "P", "S" };

        // Matrix with the solutions
        private int[,] _solutions =
            {
               /* Player 1 */
               /* R  P  S  */
        /* R */ { 1, 1, 2 },
        /* P */ { 2, 1, 1 }, /* Player 2 */
        /* S */ { 1, 2, 1 }
        };

        /// <summary>
        /// Gets the first place in a Tournament
        /// </summary>
        /// <param name="tournament">Tournament where the Players plays</param>
        /// <returns>The Player who won the first place</returns>
        public Player getTournamentWinner(Tournament tournament)
        {
            return getFirstAndSecondPlace(tournament).ElementAt(0);
        }

        /// <summary>
        /// Gets the first and second place in a Tournament
        /// </summary>
        /// <param name="tournament">Tournament where the Players plays</param>
        /// <returns>A list of Players: the first and second place</returns>
        public List<Player> getFirstAndSecondPlace(Tournament tournament)
        {
            List<Player> players = new List<Player>();

            if (tournament.ListOfTournaments.Count > 0)
            {
                for (int i = 0; i < tournament.ListOfTournaments.Count; i++)
                {
                    players.AddRange(getFirstAndSecondPlace(tournament.ListOfTournaments.ElementAt(i)));
                }

                if (players.Count == 2)
                {
                    return players;
                }

                List<Player> winners = new List<Player>();
                for (int j = 0; j < players.Count; j += 2)
                {
                    winners.Add(players.ElementAt(j));
                }
                Tournament temp = new Tournament();
                temp.ListOfPlayers = winners;
                players = getFirstAndSecondPlace(temp);
            }
            else if (tournament.ListOfPlayers.Count > 0)
            {
                players.Add(getMatchWinner(tournament.ListOfPlayers));

                if (players.ElementAt(0) == tournament.ListOfPlayers.ElementAt(0))
                {
                    players.Add(tournament.ListOfPlayers.ElementAt(1));
                }
                else
                {
                    players.Add(tournament.ListOfPlayers.ElementAt(0));
                }
            }

            return players;
        }

        /// <summary>
        /// Checks validations and gets the winner between 2 players
        /// </summary>
        /// <param name="players">List of players</param>
        /// <returns>The winner player</returns>
        public Player getMatchWinner(List<Player> players)
        {
            // Check that there're only 2 players.
            if (!(players.Count == 2))
            {
                throw new InvalidAmountOfPlayersException();
            }

            // Get the players and their strategies.
            Player firstPlayer = players.First();
            Player secondPlayer = players.Last();
            int fistPlayerStrategy = Array.IndexOf(_strategies, firstPlayer.WinnerStrategy.ToUpper());
            int secondPlayerStrategy = Array.IndexOf(_strategies, secondPlayer.WinnerStrategy.ToUpper());

            // Check that the strategies exists.
            if ((fistPlayerStrategy < 0) | (secondPlayerStrategy < 0))
            {
                throw new StrategyNotFoundException();
            }

            // Get the winner and returns it.
            int result = (int)_solutions.GetValue(secondPlayerStrategy, fistPlayerStrategy);
            if (result == 1)
            {
                return firstPlayer;
            }
            else
            {
                return secondPlayer;
            }
        }

        /// <summary>
        /// Decode the entry string getting its Tournaments and Players.
        /// </summary>
        /// <param name="encodeString">Referenced encoded string</param>
        /// <returns>A Tournament with internal Tournaments or Players</returns>
        public Tournament stringToTournament(ref string encodeString)
        {
            Tournament tournament = new Tournament();
            Player newPlayer = null;

            while (encodeString.Length > 0)
            {
                if (encodeString.ElementAt(0) == '[')
                {
                    // Checks for a new Tournament
                    encodeString = encodeString.Substring(1);
                    if (encodeString.ElementAt(0) == '[')
                    {
                        Tournament tempTournament = stringToTournament(ref encodeString);
                        if (tempTournament != null)
                        {
                            tournament.ListOfTournaments.Add(tempTournament);
                        }
                    }
                    else
                    {
                        encodeString = encodeString.Substring(1);
                        newPlayer = new Player();

                        // Getting the Player's Name
                        while (encodeString.ElementAt(0) != '\"')
                        {
                            newPlayer.Name += encodeString.ElementAt(0).ToString();
                            encodeString = encodeString.Substring(1);
                        }

                        encodeString = encodeString.Substring(3);

                        // Getting the Player's Strategy
                        while (encodeString.ElementAt(0) != '\"')
                        {
                            newPlayer.WinnerStrategy += encodeString.ElementAt(0).ToString();
                            encodeString = encodeString.Substring(1);
                        }

                        tournament.ListOfPlayers.Add(newPlayer);
                        newPlayer = null;
                        encodeString = encodeString.Substring(2);
                    }
                }
                else if (encodeString.ElementAt(0) == ']')
                {
                    encodeString = encodeString.Substring(1);
                    return tournament;
                }
                else
                {
                    encodeString = encodeString.Substring(1);
                }
            }
            return tournament;
        }


        private void printResult(Tournament tournament, ref string result, int level)
        {
            result += "\nLevel " + level + "\n";
            if (tournament.ListOfPlayers.Count == 0)
            {
                for (int i = 0; i < tournament.ListOfTournaments.Count; i++)
                {
                    printResult(
                        tournament.ListOfTournaments.ElementAt(i), ref result, level + 1);
                }
            }
            else
            {
                for (int i = 0; i < tournament.ListOfPlayers.Count; i++)
                {
                    result += "\nName: " + tournament.ListOfPlayers.ElementAt(i).Name +
                        " - Strategy: " + tournament.ListOfPlayers.ElementAt(i).WinnerStrategy;
                }
            }
        }

        private string printFAS(List<Player> players)
        {
            return
                "Primer lugar: " + players.ElementAt(0).Name +
                " - Estrategia: " + players.ElementAt(0).WinnerStrategy +
                "\nSegundo lugar: " + players.ElementAt(1).Name +
                " - Estrategia: " + players.ElementAt(1).WinnerStrategy;
        }
    }
}