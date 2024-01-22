using Game.Modules.Board.Balls;

namespace Game.Modules.GameMode
{
    public interface IGameMode
    {
        public void StartGame();
        
        public void MergeBalls(Ball mergedBall);
        
        public void EndGame();
    }
}
