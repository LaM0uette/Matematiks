using UnityEngine;

namespace Game.Modules.Bonus
{
    [CreateAssetMenu(fileName = "BonusData", menuName = "Game/BonusData")]
    public class BonusData : ScriptableObject
    {
        public int Id;
        public int Cost;
    }
}
