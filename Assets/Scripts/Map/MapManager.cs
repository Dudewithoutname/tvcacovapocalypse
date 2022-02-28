using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
// ReSharper disable LocalVariableHidesMember

public class MapManager : MonoBehaviour
{
    public static string Map = "Cacov";

    public static MapManager Singleton
    {
        get => _singleton;
        private set => _singleton ??= value;
    }
    private static MapManager _singleton;
    
    private Dictionary<Light2D, float> lights;
    private List<ItemSpawner> itemSpawners;

    private void Awake()
    {
        Singleton = this;
        
        GameManager.Singleton.Loader.Display("Loadujem mapu");
        var mapAsset = Resources.Load($"Maps/{Map}");
        GameManager.Singleton.Loader.Complete();
        
        GameManager.Singleton.Loader.Display("Vytváram mapu");
        Instantiate(mapAsset, null, true);
        GameManager.Singleton.Loader.Complete();
        
        lights = new Dictionary<Light2D, float>();
        itemSpawners = new List<ItemSpawner>();
        
        foreach (var light in FindObjectsOfType<Light2D>().Where(light => light.lightType != Light2D.LightType.Global && !light.gameObject.CompareTag("Player")))
        {
            lights.Add(light, light.intensity);
        }

        foreach (var spawner in FindObjectsOfType<ItemSpawner>())
        {
            itemSpawners.Add(spawner);
        }

    }
    private void OnDestroy() => _singleton = null;

    public void RespawnItems()
    {
        foreach (var spawner in itemSpawners)
        {
            spawner.TryRespawn();
        }
    }

    public void SetLights(bool on)
    {
        if (on)
        {
            foreach (var light in lights)
            {
                light.Key.intensity = light.Value;
            }   
        }
        else
        {
            foreach (var light in lights)
            {
                light.Key.intensity = 0f;
            } 
        }
    }
}
