using System;
using System.Linq;
using UnityEngine;

public class Rocket : Bullet
{
    public float Radius;
    
    protected override void onCollision(Collider2D coll)
    {
        var colliders = new Collider2D[24];
        Physics2D.OverlapCircleNonAlloc(gameObject.transform.position, Radius, colliders);
        Destroy(Instantiate(EffectLibrary.Particles["Explosion"], transform.position, Quaternion.identity), 1.1f);
        AudioBulb.PlayAudio(EffectLibrary.AudioClips["explode"], 0.5f);
        var victims = colliders.Where(victim => victim != null && victim.GetComponent<Enemy>() != null).Select(e => e.GetComponent<Enemy>());
        
        foreach (var victim in victims)
        {
            var distance = Vector2.Distance(victim.transform.position, transform.position);
            Destroy(Instantiate(EffectLibrary.Particles["Blood"], victim.transform.position, Quaternion.identity), 2f);
            victim.Health -= (int)Math.Floor(Weapon.Damage * ((Radius - distance) / Radius));
        }
        
        Destroy(gameObject);
    }
}