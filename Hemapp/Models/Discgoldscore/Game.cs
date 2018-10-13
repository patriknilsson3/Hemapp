using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
namespace Hemapp.Models.Discgoldscore
{
    public class Game
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public List<Score> Scores { get; set; }
        public List<Player> Players { get; set; }
        public Course Course { get; set; }
    }
}