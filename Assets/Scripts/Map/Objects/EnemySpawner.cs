using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private void Start() => EnemyManager.RegisterSpawner(this);

    public Enemy Spawn(Enemy enemy, uint instanceId)
    {
        var spawned = Instantiate(enemy, transform);
        spawned.InstanceId = instanceId;
        return spawned;
    }
}