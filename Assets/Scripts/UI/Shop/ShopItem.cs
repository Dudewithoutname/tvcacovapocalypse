using System;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public Weapon Weapon;
    
    public Text PriceText;
    public Text NameText;
    public Button OpenShop;
    public RawImage Icon;
    
    public void Init(Weapon _weapon)
    {
        Weapon = _weapon;
        PriceText.text = $"{_weapon.Price.value} €";
        PriceText.color = getMoneyColor();
        NameText.text = _weapon.Name;
        Icon.rectTransform.sizeDelta = new Vector2(_weapon.IconWidth, _weapon.IconHeight);
        Icon.texture = _weapon.IconTexture;
        Icon.transform.eulerAngles = new Vector3(Icon.transform.eulerAngles.x, Icon.transform.eulerAngles.y, _weapon.IconRot);
        OpenShop.onClick.AddListener(openShop);
    }

    private void OnDestroy() => OpenShop.onClick.RemoveAllListeners();
    private void openShop() => ShopManager.Singleton.SelectItem(this);
    private Color getMoneyColor() => Player.Singleton.Money >= Weapon.Price.value ? new Color(56, 232, 0, 255) : new Color(222, 5, 20, 255);
}