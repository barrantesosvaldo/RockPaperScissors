using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RockPaperScissors.CustomExceptions
{
    public class StrategyNotFoundException : Exception
    {
        public StrategyNotFoundException()
            : base("The entry strategy was not found.") { }
    }
}
