using System;

namespace Game.Modules.Bonus
{
    public static class BonusManager
    {
        #region Statements
        
        public static Action<BonusData> BonusEvent;
        
        public static BonusData CurrentBonus { get; set; }

        #endregion
    }
}
