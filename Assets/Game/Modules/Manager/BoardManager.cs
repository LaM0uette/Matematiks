using Game.Modules.Save;

namespace Game.Modules.Manager
{
    public static class BoardManager
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
                Saver.VolumeMuted.Save(value);
            }
        }
        
        public static void Initialize()
        {
            IsPressing = false;
            OngoingAction = false;
            IsLost = false;
            
            VolumeIsMute = Saver.VolumeMuted.Load();
        }
    }
}
