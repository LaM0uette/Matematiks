using Game.Modules.Board.Balls;

namespace Game.Modules.GameMode
{
    public interface IGameMode
    {
        public void StartGame();
        
        public void MergedBall(Ball mergedBall);
        
        public void EndGame();
    }
}
