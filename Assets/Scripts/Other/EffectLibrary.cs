using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectLibrary : MonoBehaviour
{
    public static Dictionary<string, AudioClip> AudioClips;
    public static Dictionary<string, GameObject> Particles;

    private void Awake()
    {
        AudioClips = new Dictionary<string, AudioClip>();
        Particles = new Dictionary<string, GameObject>();
        
        GameManager.Singleton.Loader.Display("Loadujem zvuky");
        foreach (var clip in Resources.LoadAll<AudioClip>("Sounds"))
        {
            AudioClips.Add(clip.name, clip);
        }
        GameManager.Singleton.Loader.Complete();
        
        GameManager.Singleton.Loader.Display("Loadujem particles");
        foreach (var particle in Resources.LoadAll<GameObject>("Particles"))
        {
            Particles.Add(particle.name, particle);
        }
        GameManager.Singleton.Loader.Complete();
    }

    private void OnDestroy()
    {
        AudioClips = null;
        Particles = null;
    }
}