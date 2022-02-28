using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

// ReSharper disable once ArrangeAccessorOwnerBody

public class Enemy : MonoBehaviour
{
    #region Editor Options
    public string Id;
    public SerializableDecimal MoneyFromKill;
    public byte Difficulty;
    public Collider2D Collider;
    public AIDestinationSetter AIDestination;
    
    [SerializeField] private int maxHealth;
    [SerializeField] private short damage;
    [SerializeField] private float speed;
    [SerializeField] private bool isFake;

    [SerializeField] private AIPath aiPath;
    [SerializeField] private List<AudioClip> deathSounds;
    #endregion

    public int Health
    {
        get => _health;
        set
        {
            _health = value;
            if (_health <= 0) Kill();
        }
    }
    public float Speed
    {
        get => aiPath.maxSpeed;
        set => aiPath.maxSpeed = value;
    }
    public uint InstanceId;
    
    private int _health;
    private bool isPlayerColliding;
    private Dictionary<uint, Collider2D> collidingEnemies;
    
    protected virtual void Init(){}

    private void Awake()
    {
        Health = maxHealth;
        Speed = speed;
        collidingEnemies = new Dictionary<uint, Collider2D>();
        Init();
    }
    
    private void Start()
    {
        if (!isFake) EnemyManager.Singleton.OnEnemyKilled += EnemyKilled;
        
        AIDestination.target = Player.Singleton.transform;
        InvokeRepeating(nameof(collisionTick), 0.35f, 0.35f);
    }
    
    private void OnDestroy()
    {
        if (!isFake && EnemyManager.Singleton != null)
        {
            EnemyManager.Singleton.OnEnemyKilled.Invoke(InstanceId);
            EnemyManager.Singleton.LiveEnemies.Remove(InstanceId);
        }
    }

    private void Kill()
    {
        if (deathSounds.Count > 0) AudioBulb.playAudio(deathSounds[Random.Range(0, deathSounds.Count)], 0.6f);
        Player.Singleton.Money += MoneyFromKill.value;
        Destroy(gameObject);
    }

    private void EnemyKilled(uint instanceId)
    {
        if (collidingEnemies.ContainsKey(instanceId)) collidingEnemies.Remove(instanceId);
    }
    
    private void OnCollisionEnter2D(Collision2D entity)
    {
        if (entity.collider.CompareTag("Enemy") && gameObject.layer != 9)
        {
            var enemy = entity.gameObject.GetComponent<Enemy>();
            if (collidingEnemies.ContainsKey(enemy.InstanceId)) return;
            // determinator
            if (Vector2.Distance(transform.position, AIDestination.target.position) > Vector2.Distance(enemy.transform.position, enemy.AIDestination.target.position)) return;
            
            collidingEnemies.Add(enemy.InstanceId, entity.collider);
            StartCoroutine(passEnemy(enemy));   
        }
        
        if (entity.collider.CompareTag("Player"))
        {
            isPlayerColliding = true;
            Player.Singleton.Health -= damage;
            StartCoroutine(damageTick());
        }
    }

    private void collisionTick()
    {
        foreach (var enemyPair in collidingEnemies) StartCoroutine(passEnemy(EnemyManager.Singleton.LiveEnemies[enemyPair.Key]));
    }

    private void OnCollisionExit2D(Collision2D entity)
    {
        if (entity.collider.CompareTag("Enemy") && gameObject.layer != 9)
        {
            var enemy = entity.gameObject.GetComponent<Enemy>();
            if (!collidingEnemies.ContainsKey(enemy.InstanceId)) return; 
            
            collidingEnemies.Remove(enemy.InstanceId);
        }

        if (entity.collider.CompareTag("Player"))
        {
            isPlayerColliding = false;
        }
    }
    private IEnumerator damageTick()
    {
       while (isPlayerColliding)
       {
           yield return new WaitForSeconds(0.4f);
           Player.Singleton.Health -= damage;
       }
    }

    private IEnumerator passEnemy(Enemy enemy)
    { 
        var prevPos = transform.position;
        var instanceId = enemy.InstanceId;

        yield return new WaitForSeconds(0.4f);
        if (Vector3.Distance(prevPos, transform.position) >= 0.08f) yield break;

        if (!collidingEnemies.ContainsKey(instanceId))
        {
            if(enemy != null) Physics2D.IgnoreCollision(Collider, enemy.Collider, false);
            collidingEnemies.Remove(instanceId);
            yield break;
        }
        Physics2D.IgnoreCollision(Collider, enemy.Collider, true);

        yield return new WaitForSeconds(Random.Range(0.65f, 0.82f));
        if (enemy != null) Physics2D.IgnoreCollision(Collider, enemy.Collider, false);
        collidingEnemies.Remove(instanceId);
    }
}