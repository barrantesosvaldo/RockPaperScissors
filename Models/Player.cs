using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RockPaperScissors.Models
{
    public class Player
    {
        [Key()]
        public int Id { get; set; }

        [DisplayName("Name")]
        [StringLength(50)]
        public string Name { get; set; }

        [DisplayName("Winner Strategy")]
        [Required()]
        [StringLength(50)]
        public string WinnerStrategy { get; set; }

        [DisplayName("Points")]
        [Required()]
        public int Points { get; set; }
    }
}