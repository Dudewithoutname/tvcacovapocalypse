using System;
using UnityEngine;

public class Krupica : Item
{
    public override ushort Id => 0;
    public override string Name => "Krupica";

    public void OnTriggerEnter2D(Collider2D trigger)
    {
        if (!trigger.CompareTag("Player")) return;

        var player = trigger.gameObject.GetComponentInParent<Player>();
        
        if (player.Health == player.MaxHealth) return;
        
        player.Health += 35;
        player.PlayAudio(EffectLibrary.AudioClips["krupica"]);
        
        Destroy(gameObject);
    }
}

