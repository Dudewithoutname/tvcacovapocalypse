using System.Collections;
using UnityEngine;

public class Gun : Weapon
{
    public ushort MaxAmmo;
    public float Recoil = 1f;
    public float Firerate;
    public float ReloadTime;
    public Transform ShootOrigin;

    public float BulletSpeed;
    public float BulletTravelTime;
    public Bullet Bullet;

    public ushort Ammo
    {
        get => _ammo;
        set
        {
            _ammo = value;
            UIManager.Singleton.Ammo = $"{Ammo} | {MaxAmmo}";
        }
    }
        
    private ushort _ammo;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.1f;
        Ammo = MaxAmmo;
    }

    public override void Use()
    {
        if (!IsAvailabe) return;
        if (Ammo <= 0)
        {
            Player.Singleton.PlayAudio(EffectLibrary.AudioClips["nestrelil"]);
            return;
        }
        
        var bullet = Bullet;
        bullet.Weapon = this;
        animator.Play("shoot", -1, 0f);
        Instantiate(bullet, ShootOrigin.position, Player.Singleton.Weapons.Origin.rotation);
        audioSource.Play();

        Ammo--;
        StartCoroutine(FireRateTick());
    }

    private IEnumerator FireRateTick()
    {
        IsAvailabe = false;
        yield return new WaitForSeconds(Firerate);
        IsAvailabe = true;
    }
    
    public IEnumerator ReloadTick() 
    {
        if (!IsAvailabe || Ammo >= MaxAmmo) yield break;
        
        IsAvailabe = false;
        animator.speed = 1 / ReloadTime; 
        animator.Play("reload", -1, 0f);
        yield return new WaitForSeconds(ReloadTime);
        Ammo = MaxAmmo;
        IsAvailabe = true;
        animator.speed = 1;
    }
    
}
