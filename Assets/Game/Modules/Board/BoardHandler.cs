using Game.Modules.Utils;

namespace Game.Modules.Board
{
    public static class BoardHandler
    {
        public static bool IsPressing { get; set; }
        public static bool OngoingAction { get; set; }
        public static bool IsLost { get; set; }

        private static bool _volumeIsMute;
        public static bool VolumeIsMute
        {
            get => _volumeIsMute;
            set
            {
                _volumeIsMute = value;
                Saver.VolumeIsMute.Save(value);
            }
        }
        
        public static void Initialize()
        {
            IsPressing = false;
            OngoingAction = false;
            IsLost = false;
            
            VolumeIsMute = Saver.VolumeIsMute.LoadBool();
        }
    }
}
