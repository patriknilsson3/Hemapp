using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hemapp.Models.Discgoldscore
{
    public class GameDto
    {
        public string UserId { get; set; }
        public int CourseId { get; set; }
        public int GameId { get; set; }
    }
    public class ScoreDto
    {
        public string UserId { get; set; }
        public int ScoreId { get; set; }
        public int CourseId { get; set; }
        public int GameId { get; set; }
        public int Hole { get; set; }
        public int Result { get; set; }
    }
    public class CourseDto
    {
        public string Name { get; set; }
        public string City { get; set; }
        public int NumberOfHoles { get; set; }
    }
}