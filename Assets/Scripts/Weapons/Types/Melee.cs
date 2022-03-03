using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Melee : Weapon
{
    public Transform HitPoint;
    public float Radius;
    public float SlashRate;
    public List<AudioClip> SlashSounds;
    
    public override void Use()
    {
        if (!IsAvailabe) return;

        AudioBulb.PlayAudio(SlashSounds[Random.Range(0, SlashSounds.Count)], 0.4f);
        animator.Play("swing", -1, 0f);

        var colliders = new Collider2D[16];
        Physics2D.OverlapCircleNonAlloc(gameObject.transform.position, 15f, colliders);
        
        foreach (var coll in colliders)
        {
            if (coll.CompareTag("Enemy"))
            {
                Destroy(Instantiate(EffectLibrary.Particles["Blood"], coll.transform), 2f);
                coll.GetComponent<Enemy>().Health -= Damage;
            }
        }

        StartCoroutine(SlashTick());
    }
    
    private IEnumerator SlashTick()
    {
        IsAvailabe = false;
        yield return new WaitForSeconds(SlashRate);
        IsAvailabe = true;
    }
    
    // joink brackeys tutorial
    private void OnDrawGizmos()
    {
        if(HitPoint == null) return;
        
        Gizmos.DrawWireSphere(HitPoint.position, Radius);
    }
}