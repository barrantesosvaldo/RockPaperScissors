using System.Data.Entity;

namespace RockPaperScissors.Models
{
    public class RockPaperScissorsContext : DbContext
    {
        public RockPaperScissorsContext() : base("RockPaperScissorsDatabase") { }

        public DbSet<Player> Player { get; set; }
    }
}