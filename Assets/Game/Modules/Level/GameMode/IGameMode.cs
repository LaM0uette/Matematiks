namespace Game.Modules.Level.GameMode
{
    public interface IGameMode
    {
        public void Initialize();
        public void MergeBallsUpdate(int mergedBallNumber, int countBallsSelected);
        public void MergeBallsComplete();
    }
}
