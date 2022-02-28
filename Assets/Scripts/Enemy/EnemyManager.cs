using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Singleton
    {
        get => _singleton;
        private set => _singleton ??= value;
    }
    private static EnemyManager _singleton;
    
    public onEnemyKilled OnEnemyKilled;
    public List<EnemySpawner> Spawners;
    public Dictionary<string, Enemy> EnemyList;
    public AudioBulb AudioBulb;
    
    public Dictionary<uint, Enemy> LiveEnemies;
    private uint nextInstanceId;
    
    private void Awake()
    {
        Singleton = this;
        Spawners = new List<EnemySpawner>();
        EnemyList = new Dictionary<string, Enemy>();
        LiveEnemies = new Dictionary<uint, Enemy>();
        nextInstanceId = 1;

        GameManager.Singleton.Loader.Display("Loadujem nepriatelov");
        foreach (var enemy in Resources.LoadAll<Enemy>("Enemies"))
        {
            EnemyList.Add(enemy.Id, enemy);
            Debug.Log($"[EnemyManager] Loaded {enemy.Id}"); 
        }
        GameManager.Singleton.Loader.Complete();

        Physics2D.IgnoreLayerCollision(9, 8, true);
    }
    private void OnDestroy() => _singleton = null;
    
    #region API
    public static void RegisterSpawner(EnemySpawner spawner) => Singleton.Spawners.Add(spawner);

    public static void SpawnEnemy(string name) => SpawnEnemy(name, Random.Range(0, Singleton.Spawners.Count));

    public static void SpawnEnemy(string name, int spawnerIndex)
    {
        if (!Singleton.EnemyList.ContainsKey(name)) return;
        
        var enemy = Singleton.Spawners[spawnerIndex].Spawn(Singleton.EnemyList[name], Singleton.nextInstanceId);
        Singleton.LiveEnemies.Add(Singleton.nextInstanceId, enemy);

        Singleton.nextInstanceId++;
    }
    
    public static void SpawnEnemy(string name, Vector2 position)
    {
        if (!Singleton.EnemyList.ContainsKey(name)) return;
        
        var enemy = Instantiate(Singleton.EnemyList[name], position, Quaternion.identity, null);
        enemy.InstanceId = Singleton.nextInstanceId;
        Singleton.LiveEnemies.Add(Singleton.nextInstanceId, enemy);

        Singleton.nextInstanceId++;
    }
    #endregion
    
    public delegate void onEnemyKilled(uint instanceId);
}
