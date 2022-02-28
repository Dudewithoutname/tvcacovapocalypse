using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class EnemyPolakovic : Enemy
{
    /* Abilites
     *  - Klarinet
     *      - Shoots notes around him self
     *  - Spawn Servants
     *      - Spawn his servants around himself 4-8
     *      - Evil vlado = his servant
     *  - Spirit Race
     *      - Rush Through Walls
     *      - Add wings to him OMG :)
     *      - Like Eye of cthulu in the export mode terraria
     *      - Alpha 0.5
     *
     *  When HP is lower than 1/4
     *      - 2x speed
     *      - Saxophone rage
     */
    
    protected override void Init()
    {
        StartCoroutine(doAbility(8));
    }

    private IEnumerator doAbility(float time)
    {
        yield return new WaitForSeconds(time);
        
        while (this != null)
        {
            StartCoroutine(summonServants());
            yield return new WaitForSeconds(time);
        }
    }

    private IEnumerator summonServants()
    {
        var preSpeed = Speed;
        Speed = 0;
        
        for (var i = 0; i < 4; i++)
        {
            var pos = getServantSpawnPos(i); 
            Destroy(Instantiate(EffectLibrary.Particles["Magic"], pos, transform.rotation), 0.8f);
            EnemyManager.SpawnEnemy("evilVlado", pos);
            
            yield return new WaitForSeconds(0.25f);
        }
        
        yield return new WaitForSeconds(0.3f);
        Speed = preSpeed;
    }

    private Vector2 getServantSpawnPos(int i) => (i % 4) switch
    {
        0 => new Vector2(transform.position.x, transform.position.y + 1.5f),
        1 => new Vector2(transform.position.x, transform.position.y - 1.5f),
        2 => new Vector2(transform.position.x - 1.5f, transform.position.y),
        3 => new Vector2(transform.position.x + 1.5f, transform.position.y),
        _ => transform.position
    };
}