using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Singleton
    {
        get => _singleton;
        private set => _singleton ??= value;
    }
    private static ItemManager _singleton;

    public List<Item> Items { get; private set; }

    private void Awake()
    { 
        Singleton = this;
        Items = Resources.LoadAll<Item>("Items").OrderBy(item => item.Id).ToList();
        
        foreach (var item in Items)
        {
            Debug.Log($"[Item {item.Id}] Loaded Item: {item.Name} from asset: {item.name}!"); // WeaponClassName AssetName 
        }
    }
    private void OnDestroy() => _singleton = null;

}