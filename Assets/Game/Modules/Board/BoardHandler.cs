namespace Game.Modules.Board
{
    public static class BoardHandler
    {
        public static bool IsPressing { get; set; }
        public static bool OngoingAction { get; set; }
        public static bool IsLost { get; set; }
        
        public static void Initialize()
        {
            IsPressing = false;
            OngoingAction = false;
            IsLost = false;
        }
    }
}
