using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Singleton
    {
        get => _singleton;
        private set => _singleton ??= value;
    }
    
    private static ShopManager _singleton;

    public GameObject Content;
    public ShopItem ShopItemPrefab;
    public List<Button> SelectorButton;
    public List<Button> BuyButton;
    
    private List<Weapon> primary;
    private List<Weapon> secondary;
    private List<Weapon> melee;
    
    private List<ShopItem> currentItems;
    
    private ShopItem selectedItem;
    public Text SelectedName;
    public Text SelectedProps;
    public RawImage SelectedIcon;
    
    private void Awake()
    {
        Singleton = this;
        primary = new List<Weapon>();
        secondary = new List<Weapon>();
        melee = new List<Weapon>();

        BuyButton[1].onClick.AddListener(buyItem);
        SelectorButton[0].onClick.AddListener(() => ChangeList(WeaponType.MELEE));
        SelectorButton[1].onClick.AddListener(() => ChangeList(WeaponType.SECONDARY));
        SelectorButton[2].onClick.AddListener(() => ChangeList(WeaponType.PRIMARY));
    }

    private void OnDestroy() => _singleton = null;


    public void LoadWeapons()
    {
        foreach (var weapon in WeaponManager.Weapons)
        {
            if (!weapon.Buyable) continue;
            
            switch (weapon.Type)
            {
                case WeaponType.PRIMARY:
                    primary.Add(weapon);
                    break;
                case WeaponType.SECONDARY:
                    secondary.Add(weapon);
                    break;
                case WeaponType.MELEE:
                    melee.Add(weapon);
                    break;
            }
        }
        
        primary.Sort((a,b) => a.Price.value.CompareTo(b.Price.value));
        secondary.Sort((a,b) => a.Price.value.CompareTo(b.Price.value));
        melee.Sort((a,b) => a.Price.value.CompareTo(b.Price.value));

        currentItems = new List<ShopItem>();
        ChangeList(WeaponType.PRIMARY);
    }

    public void ChangeList(WeaponType wtype)
    {
        currentItems.ForEach(item => Destroy(item.gameObject));
        currentItems.ClearFast();

        List<Weapon> collection;
        switch (wtype)
        {
            case WeaponType.PRIMARY:
                collection = primary;
                break;
            case WeaponType.SECONDARY:
                collection = secondary;
                break;
            case WeaponType.MELEE:
                collection = melee;
                break;
            default: 
                collection = primary;
                break;
        }

        foreach (var weapon in collection)
        {
            var shopItem = Instantiate(ShopItemPrefab, Content.transform);
            shopItem.Init(weapon);
            currentItems.Add(shopItem);
        }
    }

    public void ShopTrigger()
    {
        UIManager.Singleton.IsInteractiveOpen = !gameObject.activeSelf;
        gameObject.SetActive(!gameObject.activeSelf);
    }
    
    public void SelectItem(ShopItem item)
    {
        if (selectedItem != null && item.Weapon.Id == selectedItem.Weapon.Id) return;

        selectedItem = item;
        SelectedName.text = item.Weapon.Name;
        SelectedIcon.texture = item.Weapon.IconTexture;
        SelectedIcon.rectTransform.sizeDelta = new Vector2(item.Weapon.IconWidth, item.Weapon.IconHeight);
        SelectedIcon.rectTransform.eulerAngles = new Vector3(SelectedIcon.rectTransform.eulerAngles.x, SelectedIcon.rectTransform.eulerAngles.y, item.Weapon.IconRot);
        SelectedProps.text = getItemProps(item.Weapon);
        
        var condition = Player.Singleton.Money >= item.Weapon.Price.value || Player.Singleton.Weapons.ActiveWeapon != null && Player.Singleton.Weapons.ActiveWeapon.Id != selectedItem.Weapon.Id;
        BuyButton[Convert.ToByte(!condition)].gameObject.SetActive(false);
        BuyButton[Convert.ToByte(condition)].gameObject.SetActive(true);
    }

    private void buyItem()
    {
        if (Player.Singleton.Money < selectedItem.Weapon.Price.value) return;
        if (Player.Singleton.Weapons.ActiveWeapon != null && Player.Singleton.Weapons.ActiveWeapon.Id == selectedItem.Weapon.Id) return;
        
        Player.Singleton.Money -= selectedItem.Weapon.Price.value;
        Player.Singleton.Weapons.GiveWeapon(selectedItem.Weapon.Id);
        
        BuyButton[1].gameObject.SetActive(false);
        BuyButton[0].gameObject.SetActive(true);
    }

    private string getItemProps(Weapon weapon)
    {
        var buffer = "";
        var auto = weapon.Auto ? "AUTO" : "SEMI";
        
        buffer += $"<color=#FFF17A>Poškodenie:</color> {weapon.Damage}\n";
        buffer += $"<color=#FFF17A>Typ:</color> {auto}\n";
        switch (weapon)
        {
            case Gun gun:
                buffer += $"<color=#FFF17A>Firerate:</color> {gun.Firerate} g/s\n";
                buffer += $"<color=#FFF17A>Recoil:</color> {gun.Recoil}\n";
                buffer += $"<color=#FFF17A>Zásobník:</color> {gun.MaxAmmo}\n";
                break;
            case Melee melee:
                buffer += $"<color=#FFF17A>Dosah:</color> {melee.Radius} m\n";
                buffer += $"<color=#FFF17A>Rychlost:</color> {melee.SlashRate} u/s\n";
                break;
        }

        return buffer;
    }
}