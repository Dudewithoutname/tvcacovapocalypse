using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Singleton
    {
        get => _singleton;
        private set => _singleton ??= value;
    }
    
    private static UIManager _singleton;

    public string Health
    {
        get => health.text;
        set => health.text = value;
    }
    public string Ammo
    {
        get => ammo.text;
        set => ammo.text = value;
    }
    public string Money
    {
        get => money.text;
        set => money.text = value;
    }

    public GameObject DeathScreen;
    public GameObject GUIAmmo;
    public GameObject GUIWeapon;
    public GameObject GUIShop;
    public Text WaveInfo;
    public Text WaveEnemies;
    public Button Exit;

    [SerializeField] private Text health;
    [SerializeField] private Text ammo;
    [SerializeField] private Text money;

    public bool IsInteractiveOpen;

    private void Awake()
    {
        Singleton = this;
        
        GUIWeapon.SetActive(false);
        GUIAmmo.SetActive(false);
        Exit.onClick.AddListener(() => SceneManager.LoadScene(2));
    }
    private void OnDestroy() => _singleton = null;
    
    public void Show()
    {
        GetComponent<CanvasGroup>().alpha = 1f;
    }
}
