using Game.Modules.Board.Balls;

namespace Game.Modules.GameMode
{
    public interface IGameMode
    {
        public void Initialize();
        
        public void MergeBalls(Ball mergedBall, int countBallsSelected);

        public void CheckLoose();
    }
}
