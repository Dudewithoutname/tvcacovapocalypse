using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuTab : MonoBehaviour
{
    public bool HomeAllow;
    public Vector2 HomePosition;
    public List<Button> Buttons;
    private void OnEnable()
    {
        MenuController.Singleton.HomeButton.gameObject.SetActive(HomeAllow);

        if (HomeAllow) ((RectTransform) MenuController.Singleton.HomeButton.transform).anchoredPosition = HomePosition;
        
        for (var i = 0; i < Buttons.Count; i++)
        {
            var iN = i;
            Buttons[i].onClick.AddListener(() => onButtonClicked(iN));
        }
        onOpen();
    }

    private void OnDisable()
    {
        for (var i = 0; i < Buttons.Count; i++)
        {
            Buttons[i].onClick.RemoveAllListeners();
        }
        onClose();
    }
    
    protected virtual void onOpen() {}
    protected virtual void onClose() {}
    protected virtual void onButtonClicked(int id) {}
}