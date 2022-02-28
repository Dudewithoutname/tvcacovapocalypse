using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Singleton
    {
        get => _singleton;
        private set => _singleton ??= value;
    }
    private static WeaponManager _singleton;

    public static List<Weapon> Weapons { get; private set; }

    private void Awake()
    { 
        Singleton = this;
        Weapons = Resources.LoadAll<Weapon>("Weapons").OrderBy(wpn => wpn.Id).ToList();
        
        GameManager.Singleton.Loader.Display("Loadujem zbrane");
        foreach (var weapon in Weapons)
        {
            Debug.Log($"[Weapon {weapon.Id}] Loaded Weapon: {weapon.Name} from asset: {weapon.name}!"); // WeaponClassName AssetName 
        }
        ShopManager.Singleton.LoadWeapons();
        ShopManager.Singleton.gameObject.SetActive(false);
        GameManager.Singleton.Loader.Complete();
    }
    private void OnDestroy() => _singleton = null;

}