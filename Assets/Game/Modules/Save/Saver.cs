using System.Collections.Generic;
using Game.Modules.Save.Savers;

namespace Game.Modules.Save
{
    public static class Saver
    {
        private static readonly Dictionary<string, ISaver<bool>> _boolSavers = new();

        static Saver()
        {
            _boolSavers.Add("VolumeMuted", new BoolSaver("VolumeMuted"));
        }

        public static ISaver<bool> VolumeMuted => _boolSavers["VolumeMuted"];
    }
}
