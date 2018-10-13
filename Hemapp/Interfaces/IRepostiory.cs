using Hemapp.Models.Discgoldscore;

namespace Hemapp.Interfaces
{
    interface IGameRepostiory
    {
        Game CreateGame(Game game);
        Game AddPlayer(Game game, string playerId);
        void RemoveGame(Game game);
        Game UpdateGame(Game game);
        Game AddScore(Game game, Score score);
        Game RemoveScore(Game game, Score score);
        void AddCourse(Course course);
        Course GetCourseById(int courseId);

        void Save();
        void Dispose();
        Game GetGameById(int id);
    }
}
