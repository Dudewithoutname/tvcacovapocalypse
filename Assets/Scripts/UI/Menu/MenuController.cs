using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public static MenuController Singleton
    {
        get => _singleton;
        private set => _singleton ??= value;
    }
    private static MenuController _singleton;

    public List<MenuTab> Tabs;
    public Button HomeButton;
    
    private GameObject actualTab;

    private void Awake()
    {
        Singleton = this;
        HomeButton.onClick.AddListener(() => ChangeTab(0));
    }

    private void OnDestroy() => _singleton = null;

    private void Start()
    {
        actualTab = Tabs[0].gameObject;
        actualTab.SetActive(true);
    }

    public void ChangeTab(int id)
    {
        actualTab.SetActive(false);
        actualTab = Tabs[id].gameObject;
        actualTab.SetActive(true);
    }
}
