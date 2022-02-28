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

        AudioBulb.playAudio(SlashSounds[Random.Range(0, SlashSounds.Count)], 0.4f);
        animator.Play("swing", -1, 0f);

        // ReSharper disable once Unity.PreferNonAllocApi
        var colliders = Physics2D.OverlapCircleAll(HitPoint.position, Radius);
        
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                Destroy(Instantiate(EffectLibrary.Particles["Blood"], collider.transform.position, collider.transform.rotation), 2f);
                collider.GetComponent<Enemy>().Health -= Damage;
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