using UnityEngine;

namespace Game.Modules.Board.Spawners
{
    public interface ISpawnMode
    {
        public void SpawnBall(GameObject prefab, Transform spawnTransform);
    }
}
