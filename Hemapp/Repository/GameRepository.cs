using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using Hemapp.Interfaces;
using Hemapp.Models;
using Hemapp.Models.Discgoldscore;

namespace Hemapp.Repository
{
    public class GameRepository : IGameRepostiory
    {
        private readonly ApplicationDbContext _db;
        public GameRepository()
        {
            _db = new ApplicationDbContext();
        }
        public Game CreateGame(Game game)
        {
            _db.Games.Add(game);
            Save();
            return game;
        }

        public Game AddPlayer(Game game, string playerId)
        {
            game.Players.Add(new Player {PlayerId = playerId});
            Save();
            return game;
        }

        public void RemoveGame(Game game)
        {
            _db.Games.Remove(game);
            Save();
        }

        public Game UpdateGame(Game game)
        {
            _db.Games.AddOrUpdate(game);
            Save();
            return game;
        }

        public Game AddScore(Game game, Score score)
        {
            if (game.Scores == null)
            {
                game.Scores = new List<Score>();
            }
            game.Scores.Add(score);
            Save();
            return game;
        }


        public Game RemoveScore(Game game, Score score)
        {
            game.Scores.Remove(score);
            Save();
            return game;
        }

        public void AddCourse(Course course)
        {
            _db.Courses.Add(course);
            Save();
        }

        public Course GetCourseById(int courseId)
        {
            return _db.Courses.FirstOrDefault(x => x.Id == courseId);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Game GetGameById(int id)
        {
            return _db.Games.Include(s => s.Scores).Include(c => c.Course).Include(h => h.Course.Holes).Include(p => p.Players).FirstOrDefault(x => x.Id == id);
        }
    }
}