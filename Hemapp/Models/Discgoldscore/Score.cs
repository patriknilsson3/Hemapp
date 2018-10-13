
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hemapp.Models.Discgoldscore
{
    public class Score
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Hole Hole { get; set; }
        public int Result { get; set; }
        public string PlayerId { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}