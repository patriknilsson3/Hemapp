

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Hemapp.Extensions;
using Hemapp.Models;
using Hemapp.Models.Discgoldscore;
using Hemapp.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Hemapp.Controllers
{
    public static class ErrorMessages
    {
        public const string IncorrectInput = "Felaktig input";
        public const string GameNotFound = "Spel hittades inte";
        public const string CourseNotFound = "Banan hittades inte";
        public const string HoleNotFound = "Hål hittades inte";
        public const string ScoreNotFound = "Score hittades inte";
    }
    [Authorize]
    [RoutePrefix("api/Game")]
    public class GameController : ApiController
    {
        
        private ApplicationUserManager _userManager;
        private readonly GameRepository _gameRepository;
        public GameController()
        {
            _gameRepository = new GameRepository();
        }
        public ApplicationUserManager UserManager
        {
            get => _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }
        [Route("GetGame")]
        [HttpPost]
        public async Task<IHttpActionResult> Game(GameDto gameDto)
        {
            if (gameDto == null || gameDto.GameId == 0) return BadRequest(ErrorMessages.IncorrectInput);

            var game = _gameRepository.GetGameById(gameDto.GameId);
            if (game == null) return BadRequest(ErrorMessages.GameNotFound);

            return Ok(game);
        }

        [Route("CreateGame")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateGame(GameDto gameDto)
        {
            if (gameDto == null || gameDto.CourseId == 0) return BadRequest(ErrorMessages.IncorrectInput);

            var course = _gameRepository.GetCourseById(gameDto.CourseId);
            if (course == null) return BadRequest(ErrorMessages.CourseNotFound);

            var game = new Game
            {
                Created = DateTime.Now,
                Players = new List<Player>
                {
                     new Player
                     {
                         PlayerId = gameDto.UserId
                     }
                },
                Scores = new List<Score>(),
                Course = course
            };

            _gameRepository.CreateGame(game);
            return Ok(game);
        }

        [Route("AddPlayer")]
        [HttpPost]
        public async Task<IHttpActionResult> AddPlayer(GameDto gameDto)
        {
            if (gameDto == null || gameDto.GameId == 0) return BadRequest(ErrorMessages.IncorrectInput);

            var game = _gameRepository.GetGameById(gameDto.GameId);
            if (game == null) return BadRequest(ErrorMessages.IncorrectInput);

            var playerInGame = game.Players.FirstOrDefault(x => x.PlayerId == gameDto.UserId);

            if (playerInGame == null)
            {
                game = _gameRepository.AddPlayer(game, gameDto.UserId);
            }
            return Json(game);
        }

        [Route("AddScore")]
        [HttpPost]
        public async Task<IHttpActionResult> AddScore(ScoreDto gameDto)
        {
            if (gameDto == null) return BadRequest(ErrorMessages.IncorrectInput);

            var game = _gameRepository.GetGameById(gameDto.GameId);
            if (game == null) return BadRequest(ErrorMessages.GameNotFound);

            var hole = game.Course.Holes.FirstOrDefault(x => x.Number == gameDto.Hole);
            if (hole == null) return BadRequest(ErrorMessages.HoleNotFound);

            if (gameDto.ScoreId > 0 && gameDto.Result > 0)
            {
                var existingScore = game.Scores.FirstOrDefault(x => x.Id == gameDto.ScoreId);
                if (existingScore == null) return BadRequest(ErrorMessages.ScoreNotFound);

                if (existingScore.Result == gameDto.Result) return Ok(game);

                existingScore.Result = gameDto.Result;
                existingScore.Updated = DateTime.Now;
                game = _gameRepository.UpdateGame(game);
                return Ok(game);
            }

            var score = new Score
            {
                Hole = hole,
                Result = gameDto.Result,
                PlayerId = gameDto.UserId,
                Created = DateTime.Now,
                Updated = DateTime.Now
            };

            game = _gameRepository.AddScore(game, score);
            return Ok(game);
        }
        
        [Route("AddCourse")]
        [HttpPost]
        public async Task<IHttpActionResult> AddCourse(CourseDto courseDto)
        {
            if (courseDto == null) return BadRequest(ErrorMessages.IncorrectInput);

            var course = new Course
            {
                City = courseDto.City,
                Name = courseDto.Name,
                Holes = CreateHoles(courseDto.NumberOfHoles)
            };

            _gameRepository.AddCourse(course);
            return Ok();
        }

        private static List<Hole> CreateHoles(int courseDtoNumberOfHoles)
        {
            var list = new List<Hole>();
            for (var i = 1; i < courseDtoNumberOfHoles + 1; i++)
            {
                list.Add(new Hole
                {
                    Length = 0,
                    Par = 3,
                    Number = i
                });
            }

            return list;
        }
    }
}
