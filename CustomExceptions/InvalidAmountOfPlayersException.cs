using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RockPaperScissors.CustomExceptions
{
    public class InvalidAmountOfPlayersException : Exception
    {
        public InvalidAmountOfPlayersException()
            : base("The amount of players is not correct.") { }
    }
}
