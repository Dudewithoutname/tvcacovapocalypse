using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public sealed class PlayerWeapons : MonoBehaviour
{
    public Weapon ActiveWeapon;
    public Transform Origin;
    public Transform Axis;
    
    private bool isFlipped;
    
    public void GiveWeapon(int id)
    {
        if(ActiveWeapon != null) Destroy(ActiveWeapon.gameObject);
        
        ActiveWeapon = Instantiate(WeaponManager.Weapons[id], Origin);
        
        UIManager.Singleton.GUIAmmo.SetActive(true);
        UIManager.Singleton.GUIWeapon.SetActive(true);
        
        var rImg = UIManager.Singleton.GUIWeapon.GetComponentsInChildren<RawImage>();
        rImg[2].texture = ActiveWeapon.Model.sprite.texture;

        if (Player.Singleton.Model.flipX)
        {
            ActiveWeapon.Model.flipY = true;
            if (ActiveWeapon is Gun gun) gun.ShootOrigin.transform.localPosition = new Vector2(gun.ShootOrigin.localPosition.x, -gun.ShootOrigin.localPosition.y);
        }
    }
    
    private void Awake()
    {
        Origin = GameObject.Find("Origin").GetComponent<Transform>();
        Axis = GameObject.Find("Axis").GetComponent<Transform>();
    }

    private void Start()
    {
        StartCoroutine(cor());
    }

    private IEnumerator cor()
    {
        yield return new WaitForSeconds(1f);
        GiveWeapon(2);
    }
    private void Update()
    {
        rotate();
        checkInputs();
    }

    private void checkInputs()
    {
        if (Input.GetKeyDown(KeyCode.G) && WaveManager.Singleton.RoundState == RoundState.FREETIME) ShopManager.Singleton.ShopTrigger();
        if (UIManager.Singleton.IsInteractiveOpen) return;
        
        if (ActiveWeapon != null)
        {
            if (ActiveWeapon.Auto && Input.GetKey(KeyCode.Mouse0)) ActiveWeapon.Use();
            
            if (!ActiveWeapon.Auto && Input.GetKeyDown(KeyCode.Mouse0)) ActiveWeapon.Use();
            
            if (ActiveWeapon.IsAvailabe && ActiveWeapon is Gun gun && Input.GetKeyDown(KeyCode.R)) ActiveWeapon.StartCoroutine(gun.ReloadTick());
        }
    }

    private void rotate()
    {
        var lookDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Player.Singleton.transform.position;
        lookDir.Normalize();

        var z = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        Axis.transform.rotation = Quaternion.Euler(0, 0, z);

        Player.Singleton.Model.flipX = Mathf.Abs(z) > 90;

        if (ActiveWeapon != null && !ActiveWeapon.IsAnimating)
        {
            ActiveWeapon.Model.flipY = Mathf.Abs(z) > 90;

            if (Mathf.Abs(z) > 90 && !isFlipped)
            {
                if (ActiveWeapon is Gun gun) gun.ShootOrigin.transform.localPosition = new Vector2(gun.ShootOrigin.localPosition.x, -gun.ShootOrigin.localPosition.y);
                isFlipped = true;
            }

            if (Mathf.Abs(z) <= 90 & isFlipped)
            {
                if (ActiveWeapon is Gun gun) gun.ShootOrigin.transform.localPosition = new Vector2(gun.ShootOrigin.localPosition.x, -gun.ShootOrigin.localPosition.y);
                
                isFlipped = false;
            }
        }
    }
}
