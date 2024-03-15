using System;
using Game.Modules.Bonus;

namespace Game.Modules.Manager
{
    public static class BonusManager
    {
        #region Statements
        
        public static Action<BonusData> BonusEvent;
        
        public static BonusData CurrentBonus { get; set; }

        #endregion
    }
}
